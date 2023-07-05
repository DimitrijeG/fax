from .record import Record
from ...constants import I_ATTRIBUTES, I_FMT, CODING


class IndexRecord(Record):
    def __init__(self, attributes=I_ATTRIBUTES, format=I_FMT, coding=CODING):
        super().__init__(attributes, format, coding)

    # ================= INDEX ZONE DATA MODEL =================
    def get_rec(self, idm, im, idp, ip) -> dict:
        return {"idm": idm, "im": im, "idp": idp, "ip": ip}
    
    def get_empty_rec(self) -> dict:
        return self.get_rec(idm=-1, im=-1, idp=-1, ip=-1)
