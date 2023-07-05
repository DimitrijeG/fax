package view.frame.table;

import entity.Reservation;
import manager.CoreManager;
import table.model.GuestReservationModel;
import type.enumeration.ReservationStatus;
import util.Compare;
import view.dialog.MessageDialog;

import javax.swing.*;
import java.awt.*;

public class GuestReservationTableFrame extends TableFrame {
    private final CoreManager app;
    private JButton withdrawButton;

    public GuestReservationTableFrame(CoreManager app) {
        super(new GuestReservationModel(app));
        this.app = app;
        setTitle("Moje rezervacije");
        setSize(900, 280);
    }

    @Override
    protected void initSpecificToolBar() {
        withdrawButton = new JButton("Otkaži");
        mainToolbar.add(withdrawButton, BorderLayout.NORTH);
    }

    @Override
    protected void initActions() {
        withdrawButton.addActionListener(e -> {
            int row = table.getSelectedRow();
            if (row == -1)
                MessageDialog.tableRowNotSelected();
            else {
                int id = Integer.parseInt(table.getValueAt(row, 0).toString());
                Reservation reservation = app.reservationManager.getReservation(id);

                if (reservation != null) {
                    Reservation editedReservation = reservation.copy();
                    if (!reservation.getStatus().equals(ReservationStatus.PENDING))
                        MessageDialog.ok(null, "Rezervacija mora da bude na čekanju da biste je otkazali.", "Upozorenje", JOptionPane.WARNING_MESSAGE);
                    else if (MessageDialog.yesNo(null, "Potvrda otkazivanja rezervacije.", "Izmena rezervacije", JOptionPane.QUESTION_MESSAGE)) {
                        editedReservation.setStatus(ReservationStatus.WITHDREW);
                        app.reservationManager.updateReservation(reservation.getId(), editedReservation);
                        updateTable();
                    }
                } else
                    MessageDialog.ok(null, "Nije moguce pronaći određenu rezervaciju!", "Greška", JOptionPane.ERROR_MESSAGE);
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
