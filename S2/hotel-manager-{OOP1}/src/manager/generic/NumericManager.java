package manager.generic;

import database.DataBaseConnection;

import java.io.IOException;
import java.util.*;

public abstract class NumericManager implements DataBaseConnection {
    private final LinkedHashMap<Integer, Object> data;

    protected NumericManager() {
        data = new LinkedHashMap<>();
    }

    protected NumericManager(String filepath) throws IOException {
        this();
        fetchData(filepath);
    }

    public Integer getNextId() {
        Integer nextId = getLastKey() + 1;
        updateIdLength(nextId);
        return nextId;
    }

    private Integer getLastKey() {
        Integer last = 0;
        for (Integer key : data.keySet()) {
            if (key > last)
                last = key;
        }
        return last;
    }

    protected abstract void updateIdLength(Integer nextId);

    protected Object get(Integer key) {
        return data.get(key);
    }

    protected Integer add(Object obj) {
        Integer newId = getNextId();
        data.put(newId, obj);
        return newId;
    }

    protected boolean add(Integer key, Object obj) {
        if (!data.containsKey(key)) {
            data.put(key, obj);
            return true;
        }
        return false;
    }

    protected boolean update(Integer key, Object obj) {
        if (data.containsKey(key)) {
            data.put(key, obj);
            return true;
        }
        return false;
    }

    protected boolean remove(Integer key) {
        return data.remove(key) != null;
    }

    protected boolean contains(Integer key) {
        return data.containsKey(key);
    }

    public void reset() {
        data.clear();
    }

    public ArrayList<Object> values() {
        return new ArrayList<>(data.values());
    }

    public Set<Integer> keys() {
        return data.keySet();
    }

    public List<Map.Entry<Integer, Object>> items() {
        return new ArrayList<>(data.entrySet());
    }

    public void sort(final Comparator<Object> comparator) {
        List<Map.Entry<Integer, Object>> items = items();
        items.sort((entry1, entry2) -> comparator.compare(entry1.getValue(), entry2.getValue()));
        reset();
        for (Map.Entry<Integer, Object> entry : items)
            data.put(entry.getKey(), entry.getValue());
    }
}
