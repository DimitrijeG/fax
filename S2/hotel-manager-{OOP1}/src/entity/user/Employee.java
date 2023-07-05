package entity.user;

import main.Settings;
import type.enumeration.EducationLvl;
import type.enumeration.EmployeeRole;
import type.enumeration.Gender;

import java.time.LocalDate;

public class Employee extends User {
    EmployeeRole role;
    EducationLvl educationLvl;
    int yearsOfService;
    double salary;


    public Employee() {
        super();
        role = EmployeeRole.ADMIN;
        educationLvl = EducationLvl.ZERO;
        yearsOfService = 0;
        salary = 0;
    }

    public Employee(String name, String surname, String phone, String address, String username, String password, LocalDate birthday, Gender gender, EmployeeRole role, EducationLvl educationLvl, int yearsOfService, double salary) {
        super(name, surname, phone, address, username, password, birthday, gender);
        this.role = role;
        this.educationLvl = educationLvl;
        this.yearsOfService = yearsOfService;
        this.salary = salary;
    }

    public void updateSalary() {
        double newBase = educationLvl.getCoefficient() * Settings.minimumWage;
        double yearsOfServiceBonus = (newBase / 20) * yearsOfService;
        salary = newBase + yearsOfServiceBonus;
    }

    public EmployeeRole getRole() {
        return role;
    }

    public void setRole(EmployeeRole role) {
        this.role = role;
    }

    public EducationLvl getEducationLvl() {
        return educationLvl;
    }

    public void setEducationLvl(EducationLvl educationLvl) {
        this.educationLvl = educationLvl;
    }

    public Integer getYearsOfService() {
        return yearsOfService;
    }

    public void setYearsOfService(int yearsOfService) {
        this.yearsOfService = yearsOfService;
    }

    public Double getSalary() {
        return salary;
    }

    public void setSalary(double salary) {
        this.salary = salary;
    }

    public Employee copy() {
        return new Employee(name, surname, phone, address, username, password, birthday, gender, role, educationLvl, yearsOfService, salary);
    }
}
