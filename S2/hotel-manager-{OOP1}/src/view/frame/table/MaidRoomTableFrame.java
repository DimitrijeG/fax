package view.frame.table;

import entity.Room;
import manager.CoreManager;
import table.model.MaidRoomModel;
import type.enumeration.RoomStatus;
import util.Compare;
import view.dialog.MessageDialog;

import javax.swing.*;
import java.awt.*;

public class MaidRoomTableFrame extends TableFrame {
    private final CoreManager app;
    private JButton buttonCleanup;

    public MaidRoomTableFrame(CoreManager app) {
        super(new MaidRoomModel(app));
        this.app = app;
        setTitle("Moje sobe");
        setSize(500, 280);
    }

    @Override
    protected void initSpecificToolBar() {
        ImageIcon cleanupIcon = new ImageIcon("img/edit.gif");

        buttonCleanup = new JButton("Pospremi");
        buttonCleanup.setIcon(cleanupIcon);
        mainToolbar.add(buttonCleanup, BorderLayout.NORTH);
    }

    @Override
    protected void initActions() {
        buttonCleanup.addActionListener(e -> {
            int row = table.getSelectedRow();
            if (row == -1)
                MessageDialog.tableRowNotSelected();
            else {
                int id = Integer.parseInt(table.getValueAt(row, 0).toString());
                Room room = app.roomManager.getRoom(id);

                if (room != null) {
                    Room editedRoom = room.copy();
                    if (!room.getStatus().equals(RoomStatus.CLEANUP))
                        MessageDialog.ok(null, "Možete da pospremite samo sobe za spremanje.", "Upozorenje", JOptionPane.WARNING_MESSAGE);
                    else if (MessageDialog.yesNo(null, "Potvrda spremanja sobe?", "Spremanje sobe", JOptionPane.QUESTION_MESSAGE)) {
                        editedRoom.setStatus(RoomStatus.FREE);
                        app.roomManager.updateRoom(room.getId(), editedRoom);
                        app.logger.logRoomCleanup(app.getUser().getUsername());
                        updateTable();
                    }
                } else
                    MessageDialog.ok(null, "Nije moguce pronaći određenu sobu!", "Greška", JOptionPane.ERROR_MESSAGE);
            }
        });
    }

    // Manuelni sorter - potrebno za razumevanje rada podrazumevanog sortera tabele
    @Override
    protected void sortData(int index) {
        app.roomManager.sort((o1, o2) -> {
            Room r1 = (Room) o1;
            Room r2 = (Room) o2;
            int retVal = 0;
            switch (index) {
                case 0:
                    retVal = r1.getId().compareTo(r2.getId());
                    break;
                case 1:
                    retVal = r1.getType().compareTo(r2.getType());
                    break;
                case 2:
                    retVal = r1.getStatus().compareTo(r2.getStatus());
                    break;
                case 3:
                    retVal = Compare.compare(r1.getAmenities(), r2.getAmenities());
                    break;
                default:
                    System.out.println("Too many columns to sort.");
                    System.exit(1);
            }
            return retVal * sortOrder.get(index);
        });
    }
}
