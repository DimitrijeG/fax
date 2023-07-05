from game import Game
from consts import HUMAN, AI


def main():
    g = Game()
    try:
        g.play(AI, AI, ai_depth=4)
    except Exception as e:
        print(e)


if __name__ == "__main__":
    main()
