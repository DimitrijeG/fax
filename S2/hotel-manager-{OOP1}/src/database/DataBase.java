package database;

import main.Settings;

import java.io.*;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.nio.file.StandardOpenOption;
import java.util.ArrayList;
import java.util.HashSet;

public class DataBase {

    // Returns List of trimmed token arrays
    public static ArrayList<String[]> readTokens(String filepath, HashSet<Integer> validTokenLength) throws IOException {
        ArrayList<String[]> tokens = new ArrayList<>();

        BufferedReader in = new BufferedReader(
                new InputStreamReader(
                        Files.newInputStream(Paths.get(filepath)), StandardCharsets.UTF_8));
        String line;
        while ((line = in.readLine()) != null) {
            line = line.trim();
            if (line.equals(""))
                continue;
            String[] lineTokens = line.split(Settings.delimiterA, -1);

            if (!validTokenLength.contains(lineTokens.length))
                continue;
            for (int i = 0; i < lineTokens.length; i++)
                if (!lineTokens[i].isEmpty())
                    lineTokens[i] = lineTokens[i].trim();
            tokens.add(lineTokens);
        }
        in.close();
        return tokens;
    }

    // writes tokens to file previously whipping it
    public static void writeTokens(String filepath, ArrayList<String[]> allTokens) throws IOException {
        DataBase.whipeFile(filepath);
        DataBase.writeWithOption(filepath, allTokens, StandardOpenOption.WRITE);
    }

    private static void whipeFile(String filepath) throws FileNotFoundException {
        PrintWriter writer = new PrintWriter(filepath);
        writer.print("");
        writer.close();
    }

    // appends tokens to file
    public static void appendTokens(String filepath, ArrayList<String[]> allTokens) throws IOException {
        DataBase.writeWithOption(filepath, allTokens, StandardOpenOption.APPEND);
    }

    private static void writeWithOption(String filepath, ArrayList<String[]> allTokens, StandardOpenOption option) throws IOException {
        BufferedWriter out = new BufferedWriter(
                new OutputStreamWriter(
                        Files.newOutputStream(
                                Paths.get(filepath), option), StandardCharsets.UTF_8));
        String line;
        for (String[] tokens : allTokens) {
            line = String.join(Settings.delimiterD, tokens) + "\n";
            out.flush();
            out.write(line);
        }
        out.close();
    }


    public static String[] getFiles(String dirPath) throws IOException {
        String sp = System.getProperty("file.separator");
        dirPath = validateDir(dirPath);

        String[] files = new String[]{
                "users.txt",
                "rooms.txt",
                "reservations.txt",
                "amenities.txt",
                "services.txt",
                "roomTypes.txt",
                "logs" + sp + "reservations.txt",
                "logs" + sp + "rooms.txt"
        };
        for (int i = 0; i < files.length; i++) {
            files[i] = dirPath + sp + files[i];
            validateFile(files[i]);
        }
        return files;
    }

    public static String validateDir(String path) throws IOException {
        File dir = new File(path);
        if (!dir.exists() || !dir.isDirectory())
            throw new IOException("Putanja do direktorijuma nije validna");
        return dir.getAbsolutePath();
    }

    public static void validateFile(String path) throws IOException {
        File file = new File(path);
        if (!file.exists() || !file.isFile())
            throw new IOException("Putanja do fajla nije validna");
    }
}
