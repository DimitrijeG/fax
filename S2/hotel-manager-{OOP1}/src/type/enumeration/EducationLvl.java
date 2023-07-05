package type.enumeration;

public enum EducationLvl {
    ZERO(1), FIRST(1.3), SECOND(1.5), THIRD(1.7);

    double coefficient;

    private EducationLvl() {
    }

    private EducationLvl(double i) {
        this.coefficient = i;
    }

    private final String[] descr = {"nema", "prvi", "drugi", "treći"};

    @Override
    public String toString() {
        return descr[this.ordinal()];
    }

    // getter method
    public double getCoefficient() {
        return this.coefficient;
    }
}
