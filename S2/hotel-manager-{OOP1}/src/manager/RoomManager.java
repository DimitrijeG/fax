package manager;

import database.DataBase;
import entity.Room;
import entity.user.User;
import main.Settings;
import manager.generic.NumericManager;
import type.enumeration.RoomStatus;
import util.InternalUtil;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Comparator;
import java.util.HashSet;
import java.util.TreeSet;

public class RoomManager extends NumericManager {

    public RoomManager() {
        super();
    }

    public RoomManager(String filepath) throws IOException {
        super(filepath);
    }

    @Override
    protected void updateIdLength(Integer nextId) {
        // ne bi trebalo da se desi u realnim uslovima
        Settings.roomIdLength = Math.max(String.valueOf(nextId).length(), Settings.roomIdLength);
    }

    public Room getRoom(int id) {
        return (Room) get(id);
    }

    public boolean addRoom(Room room) {
        Integer newId = room.getId();
        if (newId == 0) { // ceka se da sistem dodeli Id
            newId = getNextId();
            room.setId(newId);
        }
        return add(newId, room);
    }

    public boolean updateRoom(Integer id, Room room) {
        remove(id);
        return add(room.getId(), room);
    }

    public boolean removeRoom(int id) {
        return remove(id);
    }

    @Override
    public void fetchData(String filepath) throws IOException {
        HashSet<Integer> validTokenLength = new HashSet<>();
        validTokenLength.add(4);
        ArrayList<String[]> allTokens = DataBase.readTokens(filepath, validTokenLength);

        Room newRoom;
        for (String[] tokens : allTokens) {
            int id = InternalUtil.parseInt(tokens[0]);
            int type = InternalUtil.parseInt(tokens[1]);
            RoomStatus status = RoomStatus.valueOf(tokens[2]);
            TreeSet<Integer> amenities = InternalUtil.parseIntegerSet(tokens[3], Settings.delimiterB);

            newRoom = new Room(id, type, status, amenities);
            add(id, newRoom);
        }
    }

    @Override
    public void saveData(String filepath) throws IOException {
        ArrayList<String[]> allTokens = new ArrayList<>();

        sort(Comparator.comparing(o -> ((Room) o).getId()));
        for (Object value : values()) {
            Room room = (Room) value;
            String[] tokens = new String[]{
                    InternalUtil.toString(room.getId()),
                    InternalUtil.toString(room.getType()),
                    InternalUtil.toString(room.getStatus()),
                    InternalUtil.toString(room.getAmenities(), Settings.delimiterB)
            };
            allTokens.add(tokens);
        }
        DataBase.writeTokens(filepath, allTokens);
    }
}
