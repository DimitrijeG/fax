package view.frame.table;

import entity.Reservation;
import entity.Room;
import manager.CoreManager;
import table.model.TodaysReservationModel;
import type.enumeration.RoomStatus;
import util.Compare;
import view.dialog.CheckInDialog;
import view.dialog.MessageDialog;

import javax.swing.*;
import java.awt.*;

public class CheckInOutReservationiTableFrame extends TableFrame {
    private final CoreManager app;
    private JButton checkInButton, checkOutButton;

    public CheckInOutReservationiTableFrame(CoreManager app) {
        super(new TodaysReservationModel(app));
        this.app = app;
        setTitle("Check in/out rezervacija");
        setSize(850, 280);
    }

    @Override
    protected void initSpecificToolBar() {
        checkInButton = new JButton("Check in");
        checkOutButton = new JButton("Check out");
        mainToolbar.add(checkOutButton, BorderLayout.SOUTH);
        mainToolbar.add(checkInButton, BorderLayout.NORTH);
    }

    @Override
    protected void initActions() {
        checkInButton.addActionListener(e -> {
            int row = table.getSelectedRow();
            if (row == -1)
                MessageDialog.tableRowNotSelected();
            else {
                int id = Integer.parseInt(table.getValueAt(row, 1).toString());
                Reservation reservation = app.reservationManager.getReservation(id);

                if (reservation != null) {
                    if (!reservation.whatCheck().equals("početak"))
                        MessageDialog.ok(null, "Možete da prihvatite samo rezervacije koje počinju danas.", "Upozorenje", JOptionPane.WARNING_MESSAGE);
                    else if (!reservation.getAssignedRoom().equals(0))
                        MessageDialog.ok(null, "Rezervaciji je već dodeljena soba.", "Upozorenje", JOptionPane.WARNING_MESSAGE);
                    else {
                        new CheckInDialog(null, app, reservation);
                        updateTable();
                    }
                } else
                    MessageDialog.ok(null, "Nije moguce pronaći određenu rezervaciju!", "Greška", JOptionPane.ERROR_MESSAGE);
            }
        });
        checkOutButton.addActionListener(e -> {
            int row = table.getSelectedRow();
            if (row == -1)
                MessageDialog.tableRowNotSelected();
            else {
                int id = Integer.parseInt(table.getValueAt(row, 1).toString());
                Reservation reservation = app.reservationManager.getReservation(id);

                if (reservation != null) {
                    if (!reservation.whatCheck().equals("kraj"))
                        MessageDialog.ok(null, "Možete da radite check out samo odlazećih rezervacija.", "Upozorenje", JOptionPane.WARNING_MESSAGE);
                    else if (MessageDialog.yesNo(null, "Potvrda check out-a?", "Check out rezervacije", JOptionPane.QUESTION_MESSAGE)) {
                        int roomId = reservation.getAssignedRoom();
                        Room r = app.roomManager.getRoom(roomId);
                        r.setStatus(RoomStatus.CLEANUP);
                        app.assignRoom(r);
                        updateTable();
                    }
                } else
                    MessageDialog.ok(null, "Nije moguce pronaći određenu rezervaciju!", "Greška", JOptionPane.ERROR_MESSAGE);
            }
        });
    }

    // Manuelni sorter - potrebno za razumevanje rada podrazumevanog sortera tabele
    @Override
    protected void sortData(int index) {
        app.reservationManager.sort((o1, o2) -> {
            Reservation r1 = (Reservation) o1;
            Reservation r2 = (Reservation) o2;
            int retVal = 0;
            switch (index) {
                case 0:
                    retVal = r1.whatCheck().compareTo(r2.whatCheck());
                case 1:
                    retVal = r1.getId().compareTo(r2.getId());
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
