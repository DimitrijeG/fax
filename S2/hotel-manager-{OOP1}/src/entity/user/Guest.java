package entity.user;

import type.enumeration.Gender;

import java.time.LocalDate;

public class Guest extends User {
    String email, passport;

    public Guest() {
        super();
        email = passport = "";
    }

    public Guest(String name, String surname, String phone, String address, LocalDate birthday, Gender gender, String email, String passport) {
        super(name, surname, phone, address, email, passport, birthday, gender);
        this.email = email;
        this.passport = passport;
    }

    public Guest(String name, String surname, String phone, String address, String username, String password, LocalDate birthday, Gender gender, String email, String passport) {
        super(name, surname, phone, address, username, password, birthday, gender);
        this.email = email;
        this.passport = passport;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getPassport() {
        return passport;
    }

    public void setPassport(String passport) {
        this.passport = passport;
    }

    public Guest copy() {
        return new Guest(name, surname, phone, address, username, password, birthday, gender, email, passport);
    }
}
