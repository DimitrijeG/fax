package table.model;

import entity.user.Employee;
import manager.UserManager;

import java.util.ArrayList;

public class EmployeeModel extends TableModel {

    UserManager userManager;

    public EmployeeModel(UserManager userManager) {
        super(new String[]{
                "Korisničko<br>ime", "Ime", "Prezime", "Lozinka",
                "Telefon", "Adresa", "Datum rođ", "Pol", "Uloga",
                "Nivo stručne<br>spreme", "Staž", "Plata"
        });
        this.userManager = userManager;
    }

    @Override
    protected ArrayList<Object> getData() {
        return userManager.employees();
    }

    @Override
    public Object getValueAt(int rowIndex, int columnIndex) {
        Employee employee = (Employee) userManager.employees().get(rowIndex);
        switch (columnIndex) {
            case 0:
                return employee.getUsername();
            case 1:
                return employee.getName();
            case 2:
                return employee.getSurname();
            case 3:
                return employee.getPassword();
            case 4:
                return employee.getPhone();
            case 5:
                return employee.getAddress();
            case 6:
                return employee.getBirthday();
            case 7:
                return employee.getGender();
            case 8:
                return employee.getRole();
            case 9:
                return employee.getEducationLvl();
            case 10:
                return employee.getYearsOfService();
            case 11:
                return employee.getSalary();
            default:
                return null;
        }

    }
}
