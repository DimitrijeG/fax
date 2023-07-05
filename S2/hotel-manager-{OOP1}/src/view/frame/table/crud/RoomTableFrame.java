package view.frame.table.crud;

import entity.Room;
import manager.CoreManager;
import table.model.RoomModel;
import util.Compare;
import util.ViewUtil;
import view.dialog.MessageDialog;
import view.dialog.crud.AddEditRoomDialog;

import javax.swing.*;

public class RoomTableFrame extends CRUDTableFrame {
    private final CoreManager app;

    public RoomTableFrame(CoreManager app) {
        super(new RoomModel(app));
        this.app = app;
        setTitle("Sobe");
        setSize(620, 280);
    }

    @Override
    protected void initActions() {
        buttonAdd.addActionListener(e -> {
            new AddEditRoomDialog(null, app.roomManager, null, app.getArticleMap());
            updateTable();
        });
        buttonEdit.addActionListener(e -> {
            int row = table.getSelectedRow();
            if (row == -1)
                MessageDialog.tableRowNotSelected();
            else {
                Integer id = ViewUtil.parseInteger(table.getValueAt(row, 0).toString());
                Room room = app.roomManager.getRoom(id);

                if (room != null) {
                    Room editedRoom = room.copy();
                    new AddEditRoomDialog(null, app.roomManager, editedRoom, app.getArticleMap());
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
                Room room = app.roomManager.getRoom(id);

                if (room != null) {
                    int choice = JOptionPane.showConfirmDialog(null, "Da li ste sigurni da zelite da obrisete sobu?",
                            "Potvrda brisanja", JOptionPane.YES_NO_OPTION);
                    if (choice == JOptionPane.YES_OPTION) {
                        app.roomManager.removeRoom(id);
                        updateTable();
                    }
                } else {
                    JOptionPane.showMessageDialog(null, "Nije moguce pronaci odabranu rezervaciju!", "Greska", JOptionPane.ERROR_MESSAGE);
                }
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
