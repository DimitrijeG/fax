package util;

import entity.user.User;

import java.util.ArrayList;
import java.util.TreeSet;

public class Compare {
    public static int compare(User u1, User u2, int index) {
        switch (index) {
            case 0:
                return u1.getUsername().compareTo(u2.getUsername());
            case 1:
                return u1.getName().compareTo(u2.getName());
            case 2:
                return u1.getSurname().compareTo(u2.getSurname());
            case 3:
                return u1.getPassword().compareTo(u2.getPassword());
            case 4:
                return u1.getPhone().compareTo(u2.getPhone());
            case 5:
                return u1.getAddress().compareTo(u2.getAddress());
            case 6:
                return u1.getBirthday().compareTo(u2.getBirthday());
            case 7:
                return u1.getGender().compareTo(u2.getGender());
            default:
                System.out.println("Invalid column.");
                System.exit(1);
        }
        return 0;
    }

    public static int compare(TreeSet<Integer> ts1, TreeSet<Integer> ts2) {
        ArrayList<Integer> arr1 = new ArrayList<>(ts1);
        ArrayList<Integer> arr2 = new ArrayList<>(ts2);
        Integer size1 = arr1.size(), size2 = arr2.size();
        if (!size1.equals(size2))
            return size1.compareTo(size2);
        int retVal = 0;
        for (int i = 0; i < size1; i++) {
            retVal = arr1.get(i).compareTo(arr2.get(i));
            if (retVal != 0)
                return retVal;
        }
        return retVal;
    }
}
