package type;

public class ExistingIdException extends ValidatorException {
    public ExistingIdException() {
        super();
    }

    public ExistingIdException(String message) {
        super(message);
    }
}
