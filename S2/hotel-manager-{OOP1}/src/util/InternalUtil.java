package util;

import main.Settings;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.Map;
import java.util.TreeSet;

public class InternalUtil {

    public static String toString(Integer i) {
        return Integer.toString(i);
    }

    public static String toString(Double d) {
        return Double.toString(d);
    }

    public static String toString(LocalDate date) {
        return date.format(Settings.dateFormatter1);
    }

    public static String toString(Enum<?> e) {
        return e.name();
    }

    public static String toString(TreeSet<Integer> set, String delimiter) {
        ArrayList<String> tokens = new ArrayList<>();
        for (Integer i : set) {
            tokens.add(toString(i));
        }
        return String.join(delimiter, tokens);
    }

    public static Integer parseInt(String s) {
        if (s.isEmpty())
            return 0;
        return Integer.parseInt(s);
    }

    public static Double parseDouble(String s) {
        if (s.isEmpty())
            return 0.0;
        return Double.parseDouble(s);
    }

    public static LocalDate parseDate(String s) {
        return LocalDate.parse(s, Settings.dateFormatter1);
    }

    public static TreeSet<Integer> parseIntegerSet(String s, String delimiter) {
        TreeSet<Integer> set = new TreeSet<>();
        String[] tokens = s.split(delimiter);
        for (String token : tokens) {
            set.add(InternalUtil.parseInt(token.trim()));
        }
        return set;
    }

    public static <V, K> LinkedHashMap<V, K> invertMap(Map<K, V> map) {
        LinkedHashMap<V, K> result = new LinkedHashMap<V, K>();

        for (Map.Entry<K, V> entry : map.entrySet())
            result.put(entry.getValue(), entry.getKey());

        return result;
    }
}
