package util;

import entity.price.Price;
import main.Settings;
import type.DateRange;

import javax.swing.*;
import javax.swing.text.JTextComponent;
import java.awt.*;
import java.text.DecimalFormat;
import java.time.LocalDate;
import java.util.Collection;
import java.util.HashMap;
import java.util.Locale;
import java.util.TreeSet;

public class ViewUtil {
    public static void changeFont(Component component, Font font) {
        component.setFont(font);
        if (component instanceof Container) {
            for (Component child : ((Container) component).getComponents()) {
                changeFont(child, font);
            }
        }
    }

    public static String getText(JTextComponent component) {
        return component.getText().trim();
    }

    public static JPanel groupButtons(AbstractButton[] buttons) {
        ButtonGroup group = new ButtonGroup();
        JPanel panel = new JPanel();
        for (AbstractButton button : buttons) {
            group.add(button);
            panel.add(button);
        }
        return panel;
    }

//    CONVERT --------------------------------------------------------

    public static String toString(LocalDate date) {
        return date.format(Settings.dateFormatter2);
    }

    public static String toString(DateRange dateRange, String delimiter) {
        return toString(dateRange.getStart()) + delimiter + toString(dateRange.getEnd());
    }

    public static String toString(DateRange dateRange) {
        return toString(dateRange, " - ");
    }

    public static String toString(Double d, Integer decimals) {
        return String.format("%." + decimals.toString() + "f", d);
    }

    public static String toString(Double d) {
        return toString(d, 2);
    }

    public static Integer parseInteger(String s) {
        return Integer.parseInt(s);
    }

    public static Double parseDouble(String s) {
        return Double.parseDouble(s);
    }

    public static LocalDate parseDate(String s) {
        return LocalDate.parse(s, Settings.dateFormatter2);
    }

    public static String formatId(Integer id, Integer idLength) {
        if (id == 0)
            return "/";
        DecimalFormat idFormat = new DecimalFormat(Common.repeatString(idLength, "0"));
        return idFormat.format(id);
    }

    public static String formatString(String s, int length) {
        return String.format("%" + length + "s", s).replace(' ', '-');
    }

    public static String[] toIdArray(Collection<Integer> col, Integer idLength) {
        int i = 0, size = col.size();
        String[] arr = new String[size];
        for (Integer id : col) {
            arr[i++] = formatId(id, idLength);
        }
        return arr;
    }

    public static String[] toPriceArray(Collection<Price> col, String delimiter) {
        int i = 0, size = col.size();
        String[] arr = new String[size];
        for (Price price : col) {
            arr[i++] = toString(price.getAmount()) + delimiter + toString(price.getDateRange());
        }
        return arr;
    }

    public static String[] toPriceArray(Collection<Price> col) {
        return toPriceArray(col, " : ");
    }

    public static String[] toStringArray(TreeSet<Integer> ids, HashMap<Integer, String> articleMap) {
        String[] result = new String[ids.size()];
        int i = 0;
        for (Integer article : ids)
            result[i++] = articleMap.get(article);
        return result;
    }
}
