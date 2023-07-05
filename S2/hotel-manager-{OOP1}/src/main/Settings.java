package main;

import java.awt.*;
import java.time.format.DateTimeFormatter;

public class Settings {
    public final static double minimumWage = 35000;
    public final static DateTimeFormatter dateFormatter1 = DateTimeFormatter.ISO_LOCAL_DATE;
    public final static DateTimeFormatter dateFormatter2 = DateTimeFormatter.ofPattern("d/M/yyyy");
    public final static String delimiterA = "\\|", delimiterB = ";", delimiterC = ",", delimiterD = "|";
    public static int reservationIdLength = 6, roomIdLength = 4, articleIdLength = 3;

    public final static Font font = new Font("Verdana", Font.PLAIN, 22);
}
