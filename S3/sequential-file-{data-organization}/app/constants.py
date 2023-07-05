from pathlib import Path
import os

# APP CONSTANTS

ATTRIBUTES = ["id", "type", "date", "ammunition", "weight"]
FMT = "9s40s16s7sd"
MOD_ATTRIBUTES = ATTRIBUTES + ["status"]
MOD_FMT = FMT + "i"
I_ATTRIBUTES = ["id", "n"]
I_FMT = "9si"
CODING = "ascii"

F = 5  # Broj slogova u bloku/baketu

ROOT_DIR = Path(__file__).parent.parent



def_active_path = os.path.join(ROOT_DIR, "data/bin/active.dat")
def_ser_path = os.path.join(ROOT_DIR, "data/bin/serial.dat")
def_seq_path = os.path.join(ROOT_DIR, "data/bin/sequential.dat")

buffer_path = os.path.join(ROOT_DIR, "data/bin/buffer.dat")
def_out_path = os.path.join(ROOT_DIR, "data/out/out.dat")
data_model_path = os.path.join(ROOT_DIR, "data/model.txt")
