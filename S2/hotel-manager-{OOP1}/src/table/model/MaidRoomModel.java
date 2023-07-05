package table.model;

import entity.Room;
import main.Settings;
import manager.CoreManager;
import util.ViewUtil;

import java.util.ArrayList;

public class MaidRoomModel extends TableModel {

    CoreManager app;

    public MaidRoomModel(CoreManager app) {
        super(new String[]{"Id<br>&nbsp", "Tip", "Status", "Dodatna<br>oprema"});
        this.app = app;
    }

    @Override
    protected ArrayList<Object> getData() {
        return app.getAssignedRooms(app.getUser().getUsername());
    }

    @Override
    public Object getValueAt(int rowIndex, int columnIndex) {
        Room room = (Room) app.getAssignedRooms(app.getUser().getUsername()).get(rowIndex);
        if (room == null)
            return null;
        switch (columnIndex) {
            case 0:
                return ViewUtil.formatId(room.getId(), Settings.roomIdLength);
            case 1:
                return ViewUtil.formatId(room.getType(), Settings.articleIdLength);
            case 2:
                return room.getStatus();
            case 3:
                return ViewUtil.toStringArray(room.getAmenities(), app.getArticleMap().amenities);
            default:
                return null;
        }

    }
}
