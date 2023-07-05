from pathlib import Path
import os


# APP CONSTANTS
ATTRIBUTES = ["id", "type", "date", "ammunition", "weight", "status"]
FMT = "i40s16sidi"

I_ATTRIBUTES = ["idm", "im", "idp", "ip"]
I_FMT = "iiii"

O_ATTRIBUTES = ATTRIBUTES + ["n"]
O_FMT = FMT + "i"

CODING = "ascii"

F = 5  # Broj slogova u bloku/baketu
N = 2  # Red index stabla


# PATHS
ROOT_DIR = Path(__file__).parent.parent

active_path = os.path.join(ROOT_DIR, "data/bin/active.dat")
ser_path = os.path.join(ROOT_DIR, "data/bin/serial.dat")
seq_path = os.path.join(ROOT_DIR, "data/bin/sequential.dat")

data_model_path = os.path.join(ROOT_DIR, "data/model.txt")
