package table.model;

import entity.Room;
import main.Settings;
import manager.CoreManager;
import util.ViewUtil;

import java.util.ArrayList;

public class RoomModel extends TableModel {

    CoreManager app;

    public RoomModel(CoreManager app) {
        super(new String[]{"Id<br>&nbsp", "Tip", "Status", "Dodatna<br>oprema"});
        this.app = app;
    }

    @Override
    protected ArrayList<Object> getData() {
        return app.roomManager.values();
    }

    @Override
    public Object getValueAt(int rowIndex, int columnIndex) {
        Room room = (Room) app.roomManager.values().get(rowIndex);
        switch (columnIndex) {
            case 0:
                return ViewUtil.formatId(room.getId(), Settings.roomIdLength);
            case 1:
                return app.getArticleMap().getRoomType(room.getType());
            case 2:
                return room.getStatus();
            case 3:
                return ViewUtil.toStringArray(room.getAmenities(), app.getArticleMap().amenities);
            default:
                return null;
        }

    }
}
