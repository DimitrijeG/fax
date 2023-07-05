package view.frame.table.crud;

import entity.Reservation;
import manager.CoreManager;
import table.model.ReservationModel;
import util.Compare;
import util.ViewUtil;
import view.dialog.MessageDialog;
import view.dialog.crud.AddEditReservationDialog;

import javax.swing.*;

public class ReservationTableFrame extends CRUDTableFrame {
    private final CoreManager app;

    public ReservationTableFrame(CoreManager app) {
        super(new ReservationModel(app));
        this.app = app;
        setTitle("Rezervacije");
        setSize(1000, 280);
    }

    @Override
    protected void initActions() {
        buttonAdd.addActionListener(e -> {
            new AddEditReservationDialog(null, app, null);
        });
        buttonEdit.addActionListener(e -> {
            int row = table.getSelectedRow();
            if (row == -1)
                MessageDialog.tableRowNotSelected();
            else {
                Integer id = ViewUtil.parseInteger(table.getValueAt(row, 0).toString());
                Reservation reservation = app.reservationManager.getReservation(id);

                if (reservation != null) {
                    Reservation editedReservation = reservation.copy();
                    new AddEditReservationDialog(null, app, editedReservation);
                    updateTable();
                } else
                    MessageDialog.ok(null, "Nije moguce pronaći određenu rezervaciju!", "Greška", JOptionPane.ERROR_MESSAGE);
            }
        });
        buttonDelete.addActionListener(e -> {
            int row = table.getSelectedRow();
            if (row == -1)
                MessageDialog.tableRowNotSelected();
            else {
                int id = ViewUtil.parseInteger(table.getValueAt(row, 0).toString());
                Reservation reservation = app.reservationManager.getReservation(id);

                if (reservation != null) {
                    int choice = JOptionPane.showConfirmDialog(null, "Da li ste sigurni da zelite da obrisete rezervaciju?",
                            "Potvrda brisanja", JOptionPane.YES_NO_OPTION);
                    if (choice == JOptionPane.YES_OPTION) {
                        app.reservationManager.removeReservation(id);
                        updateTable();
                    }
                } else {
                    JOptionPane.showMessageDialog(null, "Nije moguce pronaci odabranu rezervaciju!", "Greska", JOptionPane.ERROR_MESSAGE);
                }
            }
        });
    }

    @Override
    protected void sortData(int index) {
        app.reservationManager.sort((o1, o2) -> {
            Reservation r1 = (Reservation) o1;
            Reservation r2 = (Reservation) o2;
            int retVal = 0;
            switch (index) {
                case 0:
                    retVal = r1.getId().compareTo(r2.getId());
                    break;
                case 1:
                    retVal = r1.getStatus().compareTo(r2.getStatus());
                    break;
                case 2:
                    retVal = r1.getPrice().compareTo(r2.getPrice());
                    break;
                case 3:
                    retVal = r1.getGuest().compareTo(r2.getGuest());
                    break;
                case 4:
                    retVal = r1.getGuestCount().compareTo(r2.getGuestCount());
                    break;
                case 5:
                    retVal = r1.getDateRange().compareTo(r2.getDateRange());
                    break;
                case 6:
                    retVal = r1.getRoomType().compareTo(r2.getRoomType());
                    break;
                case 7:
                    retVal = Compare.compare(r1.getEligibleRooms(), r2.getEligibleRooms());
                    break;
                case 8:
                    retVal = r1.getAssignedRoom().compareTo(r2.getAssignedRoom());
                    break;
                case 9:
                    retVal = Compare.compare(r1.getAdditionalServices(), r2.getAdditionalServices());
                    break;
                default:
                    System.out.println("Too many columns to sort." + index);
                    System.exit(1);
            }
            return retVal * sortOrder.get(index);
        });
    }
}
