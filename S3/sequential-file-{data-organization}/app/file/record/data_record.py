from .record import Record
from ...constants import ATTRIBUTES, FMT, CODING


class DataRecord(Record):
    def __init__(self, attributes=ATTRIBUTES, format=FMT, coding=CODING):
        super().__init__(attributes, format, coding)
            
    # ================= DATA MODEL =================
    def get_rec(self, id, manufactured, weight, type, model) -> dict:
        return {"id": id, "manufactured": manufactured, "weight": weight, "type": type, "model": model}
    
    def get_empty_rec(self) -> dict:
        return self.get_rec(id=-1, manufactured=0, weight=0.0, type="", model="")
