package entity;

import type.enumeration.RoomStatus;

import java.util.TreeSet;

public class Room {
    int id, type;
    RoomStatus status;
    TreeSet<Integer> amenities;

    public Room() {
        id = 0;
        type = 0;
        status = RoomStatus.FREE;
        amenities = new TreeSet<>();
    }

    public Room(int type, RoomStatus status, TreeSet<Integer> amenities) {
        this();
        this.type = type;
        this.status = status;
        this.amenities = amenities;
    }

    public Room(int id, int type, RoomStatus status, TreeSet<Integer> amenities) {
        this(type, status, amenities);
        this.id = id;
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public Integer getType() {
        return type;
    }

    public void setType(Integer type) {
        this.type = type;
    }

    public RoomStatus getStatus() {
        return status;
    }

    public void setStatus(RoomStatus status) {
        this.status = status;
    }

    public TreeSet<Integer> getAmenities() {
        return amenities;
    }

    public void setAmenities(TreeSet<Integer> amenities) {
        this.amenities = amenities;
    }

    public Room copy() {
        return new Room(id, type, status, amenities);
    }
}
