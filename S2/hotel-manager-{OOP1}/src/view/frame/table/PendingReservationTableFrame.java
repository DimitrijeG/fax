package view.frame.table;

import entity.Reservation;
import manager.CoreManager;
import table.model.PendingReservationModel;
import type.enumeration.ReservationStatus;
import util.Compare;
import view.dialog.MessageDialog;

import javax.swing.*;
import java.awt.*;

public class PendingReservationTableFrame extends TableFrame {
    private final CoreManager app;
    private JButton approveButton, declineButton;

    public PendingReservationTableFrame(CoreManager app) {
        super(new PendingReservationModel(app));
        this.app = app;
        setTitle("Rezervacije na čekanju");
        setSize(650, 280);
    }

    @Override
    protected void initSpecificToolBar() {
        approveButton = new JButton("Potvrdi");
        declineButton = new JButton("Odbij");
        mainToolbar.add(approveButton, BorderLayout.NORTH);
        mainToolbar.add(declineButton, BorderLayout.SOUTH);
    }

    @Override
    protected void initActions() {
        addActionListenerChangeStatus(
                approveButton,
                ReservationStatus.APPROVED,
                "Možete da prihvatite samo rezervacije na čekanju.",
                "Potvrda prihvatanja rezervacije?");
        addActionListenerChangeStatus(
                declineButton,
                ReservationStatus.DECLINED,
                "Možete da odbijete samo rezervacije na čekanju.",
                "Potvrda odbijanja rezervacije?");
    }

    private void addActionListenerChangeStatus(JButton button, ReservationStatus newStatus, String message1, String message2) {
        button.addActionListener(e -> {
            int row = table.getSelectedRow();
            if (row == -1)
                MessageDialog.tableRowNotSelected();
            else {
                int id = Integer.parseInt(table.getValueAt(row, 0).toString());
                Reservation reservation = app.reservationManager.getReservation(id);

                if (reservation != null) {
                    Reservation editedReservation = reservation.copy();
                    if (!reservation.getStatus().equals(ReservationStatus.PENDING))
                        MessageDialog.ok(null, message1, "Upozorenje", JOptionPane.WARNING_MESSAGE);
                    else if (MessageDialog.yesNo(null, message2, "Izmena rezervacije", JOptionPane.QUESTION_MESSAGE)) {
                        editedReservation.setStatus(newStatus);
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
