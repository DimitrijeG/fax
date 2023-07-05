package util;

public class Common {

    public static String repeatString(int n, String str) {
        return new String(new char[n]).replace("\0", str);
    }
}
