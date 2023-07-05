package manager;

import database.DataBase;
import entity.user.Employee;
import entity.user.Guest;
import entity.user.User;
import manager.generic.AlphabeticManager;
import type.enumeration.EducationLvl;
import type.enumeration.EmployeeRole;
import type.enumeration.Gender;
import util.InternalUtil;

import javax.jws.soap.SOAPBinding;
import java.io.IOException;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Comparator;
import java.util.HashSet;


public class UserManager extends AlphabeticManager {

    public UserManager() {
        super();
    }

    public UserManager(String filepath) throws IOException {
        this();
        fetchData(filepath);
    }

    public User getUser(String username) {
        return (User) get(username);
    }

    public boolean addUser(User user) {
        return add(user.getUsername(), user);
    }

    public boolean updateUser(String username, User user) {
        remove(username);
        return add(user.getUsername(), user);
    }

    public boolean removeUser(String username) {
        return remove(username);
    }

    public ArrayList<Object> guests() {
        ArrayList<Object> guests = new ArrayList<>();
        for (Object user : values()) {
            if (user instanceof Guest)
                guests.add(user);
        }
        return guests;
    }

    public ArrayList<Object> employees() {
        ArrayList<Object> employees = new ArrayList<>();
        for (Object user : values()) {
            if (user instanceof Employee)
                employees.add(user);
        }
        return employees;
    }

    @Override
    public void fetchData(String filepath) throws IOException {
        HashSet<Integer> validTokenLength = new HashSet<>();
        validTokenLength.add(10);
        validTokenLength.add(12);
        ArrayList<String[]> allTokens = DataBase.readTokens(filepath, validTokenLength);

        User newUser;
        for (String[] tokens : allTokens) {
            String name = tokens[0], surname = tokens[1], phone = tokens[2];
            String address = tokens[3], username = tokens[4], password = tokens[5];
            LocalDate birthday = InternalUtil.parseDate(tokens[6]);
            Gender gender = Gender.valueOf(tokens[7]);

            switch (tokens.length) {
                case 10:
                    String email = tokens[8], passport = tokens[9];

                    newUser = new Guest(name, surname, phone, address, username, password, birthday, gender, email, passport);
                    break;
                case 12:
                    EmployeeRole role = EmployeeRole.valueOf(tokens[8]);
                    EducationLvl educationLvl = EducationLvl.valueOf(tokens[9]);
                    int yearsOfService = InternalUtil.parseInt(tokens[10]);
                    double salary = InternalUtil.parseDouble(tokens[11]);

                    newUser = new Employee(name, surname, phone, address, username, password, birthday, gender, role, educationLvl, yearsOfService, salary);
                    break;
                default:
                    throw new ClassCastException();
            }
            add(username, newUser);
        }
    }

    @Override
    public void saveData(String filepath) throws IOException {
        ArrayList<String[]> allTokens = new ArrayList<>();

        sort(Comparator.comparing(o -> ((User) o).getUsername()));
        for (Object value : values()) {
            User user = (User) value;
            ArrayList<String> tokenList = new ArrayList<>();
            tokenList.add(user.getName());
            tokenList.add(user.getSurname());
            tokenList.add(user.getPhone());
            tokenList.add(user.getAddress());
            tokenList.add(user.getUsername());
            tokenList.add(user.getPassword());
            tokenList.add(InternalUtil.toString(user.getBirthday()));
            tokenList.add(user.getGender().name());
            if (user instanceof Guest) {
                tokenList.add(((Guest) user).getEmail());
                tokenList.add(((Guest) user).getPassport());
            } else {
                tokenList.add(
                        InternalUtil.toString(
                                ((Employee) user).getRole()));
                tokenList.add(
                        InternalUtil.toString(
                                ((Employee) user).getEducationLvl()));
                tokenList.add(
                        InternalUtil.toString(
                                ((Employee) user).getYearsOfService()));
                tokenList.add(
                        InternalUtil.toString(
                                ((Employee) user).getSalary()));
            }
            String[] arr = new String[tokenList.size()];
            allTokens.add(tokenList.toArray(arr));
        }
        DataBase.writeTokens(filepath, allTokens);
    }
}
