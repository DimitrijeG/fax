package util;

import entity.Reservation;
import entity.Room;
import manager.CoreManager;
import type.DateRange;
import type.enumeration.ReservationStatus;

import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.TreeSet;

public class RoomFilter {
    public CoreManager app;
    private TreeSet<Integer> rooms;

    public RoomFilter() throws IOException {
        app = new CoreManager();
        rooms = new TreeSet<>();
    }

    public RoomFilter(CoreManager app) {
        this.app = app;
        reset();
    }

    public ArrayList<Object> getRoomTypes() {
        ArrayList<Object> result = new ArrayList<>();
        HashMap<Integer, ArrayList<Room>> roomTypes = new HashMap<>();

        for (Integer roomId : rooms) {
            Room room = app.roomManager.getRoom(roomId);
            if (roomTypes.containsKey(room.getType()))
                roomTypes.get(room.getType()).add(room);
            else {
                ArrayList<Room> temp = new ArrayList<>();
                temp.add(room);
                roomTypes.put(room.getType(), temp);
            }
        }

        for (ArrayList<Room> groupedRooms : roomTypes.values()) {
            Room bestRoom = groupedRooms.get(0);
            for (Room r : groupedRooms) {
                if (r.getAmenities().size() > bestRoom.getAmenities().size())
                    bestRoom = r;
            }
            result.add(bestRoom);
        }
        return result;
    }

    public void reduceRooms(DateRange range, TreeSet<Integer> amenities) {
        for (Object obj : app.reservationManager.values()) {
            Reservation res = (Reservation) obj;
            if (res.getStatus().equals(ReservationStatus.APPROVED) &&
                    range.overlaps(res.getDateRange()) &&
                    res.getEligibleRooms().size() == 1)
                rooms.remove(res.getEligibleRooms().iterator().next());
        }
        rooms.removeIf(roomId -> !app.roomManager.getRoom(roomId).getAmenities().containsAll(amenities));
    }

    public void reduceRooms(Integer type, DateRange range, TreeSet<Integer> amenities) {
        reduceRooms(range, amenities);
        rooms.removeIf(roomId -> !app.roomManager.getRoom(roomId).getType().equals(type));
    }

    public TreeSet<Integer> getRooms() {
        return rooms;
    }

    public void reset() {
        rooms = new TreeSet<>();
        for (Object obj : app.roomManager.values())
            rooms.add(((Room) obj).getId());
    }
}
