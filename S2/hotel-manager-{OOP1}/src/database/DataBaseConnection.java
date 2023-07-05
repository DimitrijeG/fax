package database;

import java.io.IOException;

public interface DataBaseConnection {
    public abstract void fetchData(String filepath) throws IOException;

    public abstract void saveData(String filepath) throws IOException;
}
