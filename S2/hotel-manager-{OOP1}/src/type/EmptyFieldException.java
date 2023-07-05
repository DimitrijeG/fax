package type;

public class EmptyFieldException extends ValidatorException {
    public EmptyFieldException() {
        super();
    }

    public EmptyFieldException(String message) {
        super(message);
    }
}
