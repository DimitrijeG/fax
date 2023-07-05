package table.model;

import entity.user.Guest;
import manager.UserManager;

import java.util.ArrayList;

public class GuestModel extends TableModel {

    UserManager userManager;

    public GuestModel(UserManager userManager) {
        super(new String[]{
                "Korisničko<br>ime", "Ime", "Prezime", "Lozinka",
                "Telefon", "Adresa", "Datum rođ", "Pol",
                "Email", "Broj pasoša"
        });
        this.userManager = userManager;
    }

    @Override
    protected ArrayList<Object> getData() {
        return userManager.guests();
    }

    @Override
    public Object getValueAt(int rowIndex, int columnIndex) {
        Guest guest = (Guest) userManager.guests().get(rowIndex);
        switch (columnIndex) {
            case 0:
                return guest.getUsername();
            case 1:
                return guest.getName();
            case 2:
                return guest.getSurname();
            case 3:
                return guest.getPassword();
            case 4:
                return guest.getPhone();
            case 5:
                return guest.getAddress();
            case 6:
                return guest.getBirthday();
            case 7:
                return guest.getGender();
            case 8:
                return guest.getEmail();
            case 9:
                return guest.getPassport();
            default:
                return null;
        }
    }
}
