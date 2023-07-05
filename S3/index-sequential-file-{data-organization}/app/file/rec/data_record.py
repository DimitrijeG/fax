from .record import Record
from ...constants import ATTRIBUTES, FMT, CODING


class DataRecord(Record):
    def __init__(self, attributes=ATTRIBUTES, format=FMT, coding=CODING):
        super().__init__(attributes, format, coding)
            
    # ================= DATA MODEL =================
    def get_rec(self, id, type, date, ammunition, weight, status) -> dict:
        return {"id": id, "type": type, "date": date, "ammunition": ammunition, "weight": weight, "status": status}
    
    def get_empty_rec(self) -> dict:
        return self.get_rec(id=-1, type="", date="", ammunition=-1, weight=0.0, status=0)
