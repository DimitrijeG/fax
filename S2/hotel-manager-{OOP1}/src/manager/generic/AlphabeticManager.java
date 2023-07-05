package manager.generic;

import database.DataBaseConnection;

import java.io.IOException;
import java.util.*;

public abstract class AlphabeticManager implements DataBaseConnection {
    private final LinkedHashMap<String, Object> data;

    protected AlphabeticManager() {
        data = new LinkedHashMap<>();
    }

    protected AlphabeticManager(String filepath) throws IOException {
        this();
        fetchData(filepath);
    }

    protected Object get(String key) {
        return data.get(key);
    }

    protected boolean add(String key, Object obj) {
        if (!data.containsKey(key)) {
            data.put(key, obj);
            return true;
        }
        return false;
    }

    protected boolean update(String key, Object obj) {
        if (data.containsKey(key)) {
            data.put(key, obj);
            return true;
        }
        return false;
    }


    protected boolean remove(String key) {
        return data.remove(key) != null;
    }

    protected boolean contains(String key) {
        return data.containsKey(key);
    }

    public void reset() {
        data.clear();
    }

    public ArrayList<Object> values() {
        return new ArrayList<>(data.values());
    }

    public Set<String> keys() {
        return data.keySet();
    }

    public List<Map.Entry<String, Object>> items() {
        return new ArrayList<>(data.entrySet());
    }

    public void sort(final Comparator<Object> comparator) {
        List<Map.Entry<String, Object>> items = items();
        items.sort((entry1, entry2) -> comparator.compare(entry1.getValue(), entry2.getValue()));
        reset();
        for (Map.Entry<String, Object> entry : items)
            data.put(entry.getKey(), entry.getValue());
    }
}
