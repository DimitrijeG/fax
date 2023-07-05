from .record import Record
from ...constants import COMMIT_ATTRIBUTES, COMMIT_FMT, CODING


class CommitRecord(Record):
    def __init__(self, attributes=COMMIT_ATTRIBUTES, format=COMMIT_FMT, coding=CODING):
        super().__init__(attributes, format, coding)

    # ================= COMMIT DATA MODEL =================
    def get_rec(self, id, manufactured, weight, type, model, status) -> dict:
        return {"id": id, "manufactured": manufactured, "weight": weight, "type": type, "model": model, "status": status}
    
    def get_empty_rec(self) -> dict:
        return self.get_rec(id=-1, manufactured=0, weight=0.0, type="", model="", status=0)
