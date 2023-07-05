package util;

import main.Settings;
import type.DateException;
import type.EmptyFieldException;
import type.ExistingIdException;
import type.ValidatorException;

import java.time.DateTimeException;
import java.time.LocalDate;
import java.util.Collection;
import java.util.Set;

public class Validate {
    public static void notEmpty(String s) throws EmptyFieldException {
        if (s.isEmpty()) throw new EmptyFieldException("Niste popunili obavezna polja.");
    }

    public static void integer(String s) throws ValidatorException {
        notEmpty(s);
        try {
            int i = Integer.parseInt(s);
            if (i < 0) throw new NumberFormatException();
        } catch (NumberFormatException e) {
            throw new ValidatorException("Polje predviđeno za broj treba da bude pozitivan ceo broj.");
        }
    }

    public static void price(String s) throws ValidatorException {
        notEmpty(s);
        try {
            double d = Double.parseDouble(s);
            if (d < 0) throw new NumberFormatException();
        } catch (NumberFormatException e) {
            throw new ValidatorException("Polje predviđeno za cenu treba da bude pozitivan decimalan broj.");
        }
    }

    public static void localDate(String s) throws ValidatorException {
        notEmpty(s);
        try {
            LocalDate.parse(s, Settings.dateFormatter2);
        } catch (DateTimeException e) {
            throw new DateException("Ispravan format datuma je dd/mm/yyyy.");
        }
    }

    private static boolean key(Object k, Collection<?> keys) throws IllegalArgumentException {
        return keys.contains(k);
    }

    private static boolean notKey(Object k, Set<?> keys) throws IllegalArgumentException {
        return !key(k, keys);
    }

    public static void id(String s) throws ValidatorException {
        integer(s);
    }

    public static void newId(String s, Set<Integer> ids, String message) throws ValidatorException {
        id(s);
        if (key(ViewUtil.parseInteger(s), ids))
            throw new ExistingIdException(message);
    }

    public static void newId(String s, Set<Integer> ids) throws ValidatorException {
        newId(s, ids, "Unesen id već postoji u sistemu.");
    }

    public static void existingId(String s, Set<Integer> ids, String message) throws ValidatorException {
        id(s);
        if (!key(ViewUtil.parseInteger(s), ids))
            throw new ExistingIdException(message);
    }

    public static void existingUsername(String s, Set<String> ids, String message) throws ValidatorException {
        notEmpty(s);
        if (!key(s, ids))
            throw new ExistingIdException(message);
    }

    public static void newUsername(String s, Collection<String> ids, String message) throws ValidatorException {
        notEmpty(s);
        if (key(s, ids))
            throw new ExistingIdException(message);
    }


    public static void username(String s) throws ValidatorException {
        notEmpty(s);
        if (s.length() < 4) throw new ValidatorException("Neispravno korisničko ime. Minimum 4 karaktera.");
    }

    public static void username(String s, Set<String> usernames) throws ValidatorException {
        username(s);
        try {
            key(s, usernames);
        } catch (IllegalArgumentException e) {
            throw new ValidatorException("Uneseno korisničko ime je zauzeto.");
        }
    }

    public static void password(String s) throws ValidatorException {
        notEmpty(s);
        if (s.length() < 6) throw new ValidatorException("Neispravna lozinka. Minimum 6 karaktera.");
    }

    public static void email(String s) throws ValidatorException {
        notEmpty(s);
    }

    public static void passport(String s) throws ValidatorException {
        notEmpty(s);
    }
}
