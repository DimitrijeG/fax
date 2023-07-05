package view.frame.table.crud;

import entity.user.Guest;
import manager.UserManager;
import table.model.GuestModel;
import util.Compare;
import view.dialog.MessageDialog;
import view.dialog.crud.AddEditGuestDialog;

import javax.swing.*;

public class GuestTableFrame extends CRUDTableFrame {
    private final UserManager userManager;

    public GuestTableFrame(UserManager userManager) {
        super(new GuestModel(userManager));
        this.userManager = userManager;
        setTitle("Gosti");
        setSize(1000, 280);
    }

    @Override
    protected void initActions() {
        buttonAdd.addActionListener(e -> {
            new AddEditGuestDialog(null, userManager, null);
            updateTable();
        });
        buttonEdit.addActionListener(e -> {
            int row = table.getSelectedRow();
            if (row == -1)
                MessageDialog.tableRowNotSelected();
            else {
                String username = table.getValueAt(row, 0).toString();
                Guest user = (Guest) userManager.getUser(username);

                if (user != null) {
                    Guest editedUser = user.copy();
                    new AddEditGuestDialog(null, userManager, editedUser);
                    updateTable();
                } else
                    MessageDialog.ok(null, "Nije moguce pronaći određenog korisnika!", "Greška", JOptionPane.ERROR_MESSAGE);
            }
        });
        buttonDelete.addActionListener(e -> {
            int row = table.getSelectedRow();
            if (row == -1)
                MessageDialog.tableRowNotSelected();
            else {
                String username = table.getValueAt(row, 0).toString();
                Guest user = (Guest) userManager.getUser(username);

                if (user != null) {
                    int choice = JOptionPane.showConfirmDialog(null, "Da li ste sigurni da zelite da obrisete gosta?",
                            user.getName() + " " + user.getSurname() + " - Potvrda brisanja", JOptionPane.YES_NO_OPTION);
                    if (choice == JOptionPane.YES_OPTION) {
                        userManager.removeUser(user.getUsername());
                        updateTable();
                    }
                } else {
                    JOptionPane.showMessageDialog(null, "Nije moguce pronaci odabranog gosta!", "Greska", JOptionPane.ERROR_MESSAGE);
                }
            }
        });
    }

    @Override
    protected void sortData(int index) {
        userManager.sort((o1, o2) -> {
            if (!(o1 instanceof Guest) || !(o2 instanceof Guest))
                return 0;
            Guest g1 = (Guest) o1;
            Guest g2 = (Guest) o2;
            int retVal = 0;
            if (index < 8)
                retVal = Compare.compare(g1, g2, index);
            else
                switch (index) {
                    case 8:
                        retVal = g1.getEmail().compareTo(g2.getEmail());
                        break;
                    case 9:
                        retVal = g1.getPassport().compareTo(g2.getPassport());
                        break;
                    default:
                        System.out.println("Too many columns to sort.");
                        System.exit(1);
                }
            return retVal * sortOrder.get(index);
        });
    }
}
