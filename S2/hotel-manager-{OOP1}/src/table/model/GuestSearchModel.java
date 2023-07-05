package table.model;

import entity.Room;
import main.Settings;
import manager.CoreManager;
import type.DateRange;
import util.RoomFilter;
import util.ViewUtil;

import java.util.ArrayList;
import java.util.TreeSet;

public class GuestSearchModel extends TableModel {

    RoomFilter roomFilter;

    public GuestSearchModel(RoomFilter roomFilter) {
        super(new String[]{"Tip sobe", "Dodatna oprema"});
        this.roomFilter = roomFilter;
    }

    @Override
    protected ArrayList<Object> getData() {
        return roomFilter.getRoomTypes();
    }

    @Override
    public Object getValueAt(int rowIndex, int columnIndex) {
        Room room = (Room) roomFilter.getRoomTypes().get(rowIndex);
        if (room == null)
            return null;
        switch (columnIndex) {
            case 0:
                return roomFilter.app.getArticleMap().getRoomType(room.getType());
            case 1:
                return ViewUtil.toStringArray(room.getAmenities(), roomFilter.app.getArticleMap().amenities);
            default:
                return null;
        }

    }
}
