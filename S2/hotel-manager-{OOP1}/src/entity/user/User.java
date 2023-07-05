package entity.user;

import type.enumeration.Gender;

import java.time.LocalDate;

public abstract class User {
    String name, surname, phone, address, username, password;
    LocalDate birthday;
    Gender gender;

    public User() {
        name = surname = phone = address = username = password = "";
        birthday = LocalDate.of(1800, 1, 1);
        gender = Gender.OTHER;
    }

    public User(String name, String surname, String phone, String address, String username, String password, LocalDate birthday, Gender gender) {
        this.name = name;
        this.surname = surname;
        this.phone = phone;
        this.address = address;
        this.username = username;
        this.password = password;
        this.birthday = birthday;
        this.gender = gender;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getSurname() {
        return surname;
    }

    public void setSurname(String surname) {
        this.surname = surname;
    }

    public String getPhone() {
        return phone;
    }

    public void setPhone(String phone) {
        this.phone = phone;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public LocalDate getBirthday() {
        return birthday;
    }

    public void setBirthday(LocalDate birthday) {
        this.birthday = birthday;
    }

    public Gender getGender() {
        return gender;
    }

    public void setGender(Gender gender) {
        this.gender = gender;
    }
}
