package type.enumeration;

public enum RoomStatus {
    TAKEN(0), CLEANUP(1), FREE(2);

    int status;

    private RoomStatus() {
    }

    private RoomStatus(int i) {
        this.status = i;
    }

    private final String[] descr = {"zauzeta", "spremanje", "slobodna"};

    @Override
    public String toString() {
        return descr[this.ordinal()];
    }
}
