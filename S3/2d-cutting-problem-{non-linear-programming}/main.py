import os
os.environ['PYGAME_HIDE_SUPPORT_PROMPT'] = "hide"

from operator import itemgetter
from datetime import datetime
import numpy as np
import random
import pygame
import time


# ------------ GLOBALS ------------
UNIBLUE    = '\33[34m'
UNIEND     = '\33[0m'
CBLUE      = (0, 102, 255)
CLIGHTBLUE = (153, 204, 255)
CGREY      = (233, 242, 251)

W      = 0
H      = 0
R      = []
DECODED_CHROMOSOMES = {}
SOLUTION = False

pygame_scaling_fact = 2
# --------------------------------



def cost_func(chromosome):
    rectangles_sum = 0
    for r in chromosome:
        # sabiranje površina manjih pravougaonika
        rectangles_sum += r[2]*r[3]

    return rectangles_sum / (W*H)


# hromozom nastaje od stringa "1" dužine N čijih se N/4 karaktera postavlja na 0
# ovo se pokazalo kao najbolje početno rešenje zato što većina problema
# podrazumeva potpuno ili skoro potpuno iskorišćavanje materijala za sečenje
def generate_chromosome(N: int) -> str:
    s = "1" * N
    for _ in range(N//4):
        x = random.randrange(N)
        s = s[:x] + "0" + s[x+1:]
    return s


# pocetna populacija binarnih hromozoma
def generate_inital_chromosomes(population_size=100) -> list:
   return [generate_chromosome(len(R)) for _ in range(population_size)]


# rangira hromozome prema fitnessu
def rank_chromosomes(chromosomes):
    global DECODED_CHROMOSOMES
    costs = {}

    for c in chromosomes:
        # provera da li je hromozom vec dekodiran zbog optimizacije
        if c in DECODED_CHROMOSOMES:
            costs[c] = DECODED_CHROMOSOMES[c]
        else:
            cost = decodeChromosome(c)[1]
            costs[c] = cost
            DECODED_CHROMOSOMES[c] = cost
    
    # sortiranje recnika
    ranked = dict(sorted(costs.items(), key = lambda item: item[1], reverse=False))
    return list(ranked.keys()), list(ranked.values())


# generisanje parova koji će proizvesti decu
def roulette_selection(parents) -> list[tuple]:
    pairs = []
    i = 0
    for i in range(0, len(parents), 2):
        weights=[]

        for i in range(len(parents)):
            weights.append((i+1) * random.random())

        if (weights[0] >= weights[1]):
            maxInd1, maxInd2 = 0, 1
        else:
            maxInd1, maxInd2 = 1, 0
        
        for i in range(2,len(parents)):
            if weights[i] > weights[maxInd1]:
                maxInd2 = maxInd1
                maxInd1 = i
            elif weights[i] > weights[maxInd2]:
                maxInd2 = 1
        pairs.append([parents[maxInd1], parents[maxInd2]])
    return pairs


# generisanje populacije dece
def two_point_crossover(pairs: list[tuple]) -> list:
    length = len(pairs[0])
    children = []
  
    for (a, b) in pairs:  
        r1 = random.randrange(0, length)
        r2 = random.randrange(0, length)   
        if r1 < r2:
            children.append(a[:r1] + b[r1:r2] + a[r2:])
            children.append(b[:r1] + a[r1:r2] + b[r2:])
        else:
            children.append(a[:r2] + b[r2:r1] + a[r1:])
            children.append(b[:r2] + a[r2:r1] + b[r1:])
    return children


def mutation(chromosomes, rate: float) -> list:
    mutated_chromosomes = []  

    for chromosome in chromosomes:
        if random.random() < rate:
            r1 = random.randrange(0, len(chromosome) - 1)     
            c = "1"
            if chromosome[r1] == "1":
                c = "0"
            mutated_chromosomes.append(chromosome[:r1] + c + chromosome[r1+1:])        
        else:
            mutated_chromosomes.append(chromosome)
    return mutated_chromosomes


def elitis(parents, children, rate: float, population_size: int) -> list:
    old_ind_size = int(np.round(population_size*rate))
    return parents[:old_ind_size] + children[:(population_size-old_ind_size)]


# cost funkcija se ne prosleđuje već je jednoznačno definisana u kodu
# curr_best moguće prilagoditi prema problemu
def genetic(population_size, mutation_rate=0.7, elitis_rate=0.1, max_iter=100) -> str:
    curr_best = 0.96
    same_best_count = 0

    chromosomes = generate_inital_chromosomes(population_size)

    for i in range(max_iter):
        ranked_parents, costs = rank_chromosomes(chromosomes)
        best = costs[-1]

        if SOLUTION:
            print(f"Rasporedjeni svi pravougaonici, zaustavljanje algoritma ({UNIBLUE}{i}/{max_iter} iteracija{UNIEND}).")
            return ranked_parents[-1]

        pairs = roulette_selection(ranked_parents)
        children = two_point_crossover(pairs)
        chromosomes = mutation(children, mutation_rate)

        ranked_children, costs = rank_chromosomes(chromosomes)
        chromosomes = elitis(ranked_parents, ranked_children, elitis_rate, population_size)

        if best < curr_best:
            same_best_count = 0
        else:
            same_best_count += 1
        

        if same_best_count >= 10:
            print(f"Algoritam zaustavljen zbog konvergencije ({UNIBLUE}{i}/{max_iter} iteracija{UNIEND}).")
            return chromosomes[-1]
            
    print(f"Izvršen maksimalan broj iteracija od {UNIBLUE}{i+1}{UNIEND}.")
    return chromosomes[-1]


# ograničenja za svaka dva pravougaonika
def restrictions(r1, r2):
    return r1[0] + r1[2] <= W and \
        r1[1] + r1[3] <= H and \
        max(r1[0] - (r2[0] + r2[2]), \
            r2[0] - (r1[0] + r1[2]), \
            r1[1] - (r2[1] + r2[3]), \
            r2[1] - (r1[1] + r1[3])) >= 0


# generiše validnu poziciju za novi pravougaonik poštujući ograničenja
def generatePosition(r, table):
    w, h = r[2:4]

    if not table:
        return (0, 0, w, h)

    table.sort(key = itemgetter(0,1))
    for _ in range(2):
        for i in table:
            r = (i[0] + i[2], i[1], w, h)
            overlapping = False
            for j in table:
                if not restrictions(r, j):
                    overlapping = True
                    break

            if not overlapping:
                return r
            else:
                r = (i[0], i[1] + i[3], w, h)
                
                overlapping = False
                for j in table:
                    if not restrictions(r, j):
                        overlapping = True
                        break

                if not overlapping:
                    return r
                else:
                    r = (0, i[1] + i[3], w, h)
                
                    overlapping = False
                    for j in table:
                        if not restrictions(r, j):
                            overlapping = True
                            break

                    if not overlapping:
                        return r
                    else:
                        r = (i[0] + i[2], 0, w, h)
                
                        overlapping = False
                        for j in table:
                            if not restrictions(r, j):
                                overlapping = True
                                break

                        if not overlapping:
                            return r
        w, h = h, w
    return None


# generiše raspored pravougaonika i njegovu prilagođenost pomoću hromozoma
def decodeChromosome(chromosome) -> tuple[list, float]:
    table = []
    for i in range(len(chromosome)):
        if chromosome[i] =="1":
            r = R[i]
            new_r = generatePosition(r, table)
            if new_r:
                table.append(new_r)

    if len(table) == len(R):
        global SOLUTION
        SOLUTION = True
    return table, cost_func(table)


def draw_table(table, surface, cbackground=CLIGHTBLUE, cborder=CBLUE):
    for r in table:
        pygame.draw.rect(surface, cbackground, pygame.Rect(*r), 0)
        pygame.draw.rect(surface, cborder, pygame.Rect(*r),  1)


def main():
    random.seed(datetime.now().timestamp())
    
    width, height = 0, 0
    while True:
        width = input("Unesite širinu materijala: ")
        height = input("Unesite dužinu materijala: ")

        if width.isnumeric() and height.isnumeric() and int(width)>0 and int(height)>0:
            break
        print("Širina ili dužina nisu validni brojevi.")
    print("-----------------------------")

    global W, H, R
    W = int(width)
    H = int(height)

    while True:
        while True:
            rw = input("Unesite širinu pravougaonika: ")
            rh = input("Unesite dužinu pravougaonika: ")
            number = input("Unesite broj pravougaonika: ")

            if rw.isnumeric() and rh.isnumeric() and number.isnumeric() and int(number)>0 and int(rw)>0 and int(rh)>0 and int(rw)<=W and int(rh)<=H:
                break
            print("Širina, dužina ili broj pravougaonika nisu validni brojevi.")
        
        for _ in range(int(number)):
            R.append((0,0,int(rw),int(rh)))

        x = input("\nUnesite x da biste prekinuli unos.\n")
        if x.lower() == "x":
            break
    print("-----------------------------")


    # W = 400
    # H = 300
    # R = [
    #     (0, 0, 200, 40),
    #     (0, 0, 80, 70),
    #     (0, 0, 70, 170),
    #     (0, 0, 110, 40),
    #     (0, 0, 80, 80),
    #     (0, 0, 60, 130),
    #     (0, 0, 60, 40),
    #     (0, 0, 70, 50),
    #     (0, 0, 100, 50),
    #     (0, 0, 30, 70),
    #     (0, 0, 150, 100),
    #     (0, 0, 40, 120),
    #     (0, 0, 50, 100),
    #     (0, 0, 140, 80),
    #     (0, 0, 120, 60),
    #     (0, 0, 180, 90),
    #     (0, 0, 30, 60),
    #     (0, 0, 30, 90),
    #     (0, 0, 70, 60)
    # ]

    # sortiranje unetih pravougaonika opadajuće po površini
    R.sort(key = lambda v: v[2]*v[3], reverse=True)

    start = time.time()
    result = genetic(population_size=100) # pretraga rešenja
    end = time.time()
    print("Vreme genetskog algoritma: {}{}s{}".format(UNIBLUE, round(end - start, 5), UNIEND))

    table, cost = decodeChromosome(result)
    print("Procenat otpada: {}{}%{}".format(UNIBLUE, round((1-cost) * 100, 2), UNIEND))
    print("Iskorišćeno: {}{}/{}{} pravougaonika".format(UNIBLUE, len(table), len(R), UNIEND))


    # grafički prikaz rešenja
    pygame.init()
    pygame.display.set_caption("Genetic 2D cutting")
    surface = pygame.display.set_mode((W, H))
    surface.fill(CBLUE)
    draw_table(table, surface)


    # pygame prikaz prozora
    pygame.display.flip()
    running = True
    while running:
        for event in pygame.event.get():   
            if event.type == pygame.QUIT:
                running = False

main()
