package type.enumeration;

public enum EmployeeRole {
    ADMIN(0), RECEPTIONIST(1), MAID(2);

    int role;

    private EmployeeRole() {
    }

    private EmployeeRole(int i) {
        this.role = i;
    }

    private final String[] descr = {"administrator", "recepcioner", "sobarica"};

    @Override
    public String toString() {
        return descr[this.ordinal()];
    }
}
