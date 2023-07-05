package type.enumeration;

public enum ReservationStatus {
    PENDING(0), APPROVED(1), DECLINED(2), WITHDREW(3);

    int status;

    private ReservationStatus() {
    }

    private ReservationStatus(int i) {
        this.status = i;
    }

    private final String[] descr = {"na čekanju", "potvrđena", "odbijena", "otkazana"};

    @Override
    public String toString() {
        return descr[this.ordinal()];
    }
}
