package type.enumeration;

public enum Gender {
    MALE(0), FEMALE(1), OTHER(2);

    int gender;

    private Gender() {
    }

    private Gender(int i) {
        this.gender = i;
    }

    private final String[] descr = {"muško", "žensko", "drugo"};

    @Override
    public String toString() {
        return descr[this.ordinal()];
    }
}
