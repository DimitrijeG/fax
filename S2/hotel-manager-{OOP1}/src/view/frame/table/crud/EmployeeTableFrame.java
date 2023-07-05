package view.frame.table.crud;

import entity.user.Employee;
import manager.UserManager;
import table.model.EmployeeModel;
import util.Compare;
import view.dialog.MessageDialog;
import view.dialog.crud.AddEditEmployeeDialog;

import javax.swing.*;

public class EmployeeTableFrame extends CRUDTableFrame {
    private final UserManager userManager;

    public EmployeeTableFrame(UserManager userManager) {
        super(new EmployeeModel(userManager));
        this.userManager = userManager;
        setTitle("Zaposleni");
        setSize(1110, 280);
    }

    @Override
    protected void initActions() {
        buttonAdd.addActionListener(e -> {
            new AddEditEmployeeDialog(null, userManager, null);
        });
        buttonEdit.addActionListener(e -> {
            int row = table.getSelectedRow();
            if (row == -1)
                MessageDialog.tableRowNotSelected();
            else {
                String username = table.getValueAt(row, 0).toString();
                Employee user = (Employee) userManager.getUser(username);

                if (user != null) {
                    Employee editedUser = user.copy();
                    new AddEditEmployeeDialog(null, userManager, editedUser);
                    updateTable();
                } else
                    MessageDialog.ok(null, "Nije moguce pronaći određenog zaposlenog!", "Greška", JOptionPane.ERROR_MESSAGE);
            }
        });
        buttonDelete.addActionListener(e -> {
            int row = table.getSelectedRow();
            if (row == -1)
                MessageDialog.tableRowNotSelected();
            else {
                String username = table.getValueAt(row, 0).toString();
                Employee user = (Employee) userManager.getUser(username);

                if (user != null) {
                    int choice = JOptionPane.showConfirmDialog(null, "Da li ste sigurni da zelite da obrisete zaposlenog?",
                            user.getName() + " " + user.getSurname() + " - Potvrda brisanja", JOptionPane.YES_NO_OPTION);
                    if (choice == JOptionPane.YES_OPTION) {
                        userManager.removeUser(user.getUsername());
                        updateTable();
                    }
                } else {
                    JOptionPane.showMessageDialog(null, "Nije moguce pronaci odabranog zaposlenog!", "Greska", JOptionPane.ERROR_MESSAGE);
                }
            }
        });
    }

    @Override
    protected void sortData(int index) {
        userManager.sort((o1, o2) -> {
            if (!(o1 instanceof Employee) || !(o2 instanceof Employee))
                return 0;
            Employee e1 = (Employee) o1;
            Employee e2 = (Employee) o2;
            int retVal = 0;
            if (index < 8)
                retVal = Compare.compare(e1, e2, index);
            else
                switch (index) {
                    case 8:
                        retVal = e1.getRole().compareTo(e2.getRole());
                        break;
                    case 9:
                        retVal = e1.getEducationLvl().compareTo(e2.getEducationLvl());
                        break;
                    case 10:
                        retVal = e1.getYearsOfService().compareTo(e2.getYearsOfService());
                        break;
                    case 11:
                        retVal = e1.getSalary().compareTo(e2.getSalary());
                        break;
                    default:
                        System.out.println("Too many columns to sort.");
                        System.exit(1);
                }
            return retVal * sortOrder.get(index);
        });
    }
}
