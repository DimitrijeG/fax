import matplotlib.pyplot as plt
from scipy.interpolate import CubicSpline
import numpy as np

from math import floor, ceil

# --------------- cutoff 6 ---------------
def cut6():
    x = [1, 2, 3, 4, 5, 6]
    y = [57.053, 38.733, 35.498, 34.050, 35.420, 35.891]
    return (x, y)
# --------------- cutoff 7 ---------------
def cut7():
    x = [1, 2, 3, 4, 5, 6, 7]
    y = [987.459, 662.191, 596.322, 576.294, 595.186, 624.960, 652.550]
    return (x, y)
# --------------- cutoff 8 ---------------
def cut8():
    x = [1, 2, 3, 4, 5, 6, 7, 8]
    y = [54.218, 38.706, 35.185, 33.594, 31.952, 36.038, 41.378, 42.902]
    return (x, y)
# --------------- benchmark ---------------
def bench_ser():
    x = [5, 6, 7, 8]
    y = [0.00605, 0.11706, 2.13530, 86.24911]
    return (x, y)

def bench_par():
    x = [5, 6, 7, 8]
    y = [0.00247, 0.03510, 0.57629, 31.95233]
    return (x, y)
# --------------- speedup ---------------
def speedup():
    x = [5, 6, 7, 8]
    y = [2.44, 3.33, 3.71, 2.70]
    return (x, y)
# --------------- generated ---------------
def generated():
    x = [5, 6, 7, 8]
    y = [18186, 292821, 5389768, 175279198]
    return (x, y)
# ----------------------------------------
def splinify(x, y):
    cs = CubicSpline(x, y)
    xp = np.linspace(min(x), max(x), 100)
    return (xp, cs(xp))
# ----------------------------------------

plt.title('Paralelno ubrzanje')
plt.xlabel('Broj ponuđenih brojeva')
plt.ylabel('Ubrzanje')

x, y = speedup()
# y = np.log10(y)

plt.xticks(range(min(x), max(x)+1))

plt.scatter(x, y, color='blue')
x, y = splinify(x, y)
plt.plot(x, y, 'blue')

plt.show()