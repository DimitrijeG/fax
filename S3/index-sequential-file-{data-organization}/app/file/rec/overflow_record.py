from .record import Record
from ...constants import O_ATTRIBUTES, O_FMT, CODING


class OverflowRecord(Record):
    def __init__(self, attributes=O_ATTRIBUTES, format=O_FMT, coding=CODING):
        super().__init__(attributes, format, coding)
            
    # ================= DATA MODEL =================
    def get_rec(self, id, type, date, ammunition, weight, n) -> dict:
        return {"id": id, "type": type, "date": date, "ammunition": ammunition, "weight": weight, "n": n}
    
    def get_empty_rec(self) -> dict:
        return self.get_rec(id=-1, type="", date="", ammunition=-1, weight=0.0, n=-1)
