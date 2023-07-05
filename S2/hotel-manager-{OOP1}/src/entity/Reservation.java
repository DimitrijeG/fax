package entity;

import type.DateRange;
import type.enumeration.ReservationStatus;

import java.time.LocalDate;
import java.util.TreeSet;

public class Reservation {
    private int id, guestCount, assignedRoom, roomType;
    private double price;
    private TreeSet<Integer> additionalServices, eligibleRooms;
    private ReservationStatus status;
    private String guest;
    private DateRange dateRange;

    public Reservation() {
        id = guestCount = assignedRoom = roomType = 0;
        price = 0;
        additionalServices = eligibleRooms = new TreeSet<>();
        status = ReservationStatus.PENDING;
        guest = null;
        dateRange = new DateRange();
    }

    public Reservation(int guestCount, double price, TreeSet<Integer> additionalServices, ReservationStatus status, TreeSet<Integer> eligibleRooms, int assignedRoom, int roomType, String guest, DateRange dateRange) {
        this.id = 0;
        this.guestCount = guestCount;
        this.price = price;
        this.additionalServices = additionalServices;
        this.status = status;
        this.eligibleRooms = eligibleRooms;
        this.assignedRoom = assignedRoom;
        this.roomType = roomType;
        this.guest = guest;
        this.dateRange = dateRange;
    }

    public Reservation(int id, int guestCount, double price, TreeSet<Integer> additionalServices, ReservationStatus status, TreeSet<Integer> eligibleRooms, int assignedRoom, int roomType, String guest, DateRange dateRange) {
        this(guestCount, price, additionalServices, status, eligibleRooms, assignedRoom, roomType, guest, dateRange);
        this.id = id;
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public Integer getGuestCount() {
        return guestCount;
    }

    public void setGuestCount(Integer guestCount) {
        this.guestCount = guestCount;
    }

    public Double getPrice() {
        return price;
    }

    public void setPrice(Double price) {
        this.price = price;
    }

    public TreeSet<Integer> getAdditionalServices() {
        return additionalServices;
    }

    public void setAdditionalServices(TreeSet<Integer> additionalServices) {
        this.additionalServices = additionalServices;
    }

    public ReservationStatus getStatus() {
        return status;
    }

    public void setStatus(ReservationStatus status) {
        this.status = status;
    }

    public TreeSet<Integer> getEligibleRooms() {
        return eligibleRooms;
    }

    public void setEligibleRooms(TreeSet<Integer> eligibleRooms) {
        this.eligibleRooms = eligibleRooms;
    }

    public Integer getAssignedRoom() {
        return assignedRoom;
    }

    public void setAssignedRoom(Integer assignedRoom) {
        this.assignedRoom = assignedRoom;
    }

    public Integer getRoomType() {
        return roomType;
    }

    public void setRoomType(Integer roomType) {
        this.roomType = roomType;
    }

    public String getGuest() {
        return guest;
    }

    public void setGuest(String guest) {
        this.guest = guest;
    }

    public DateRange getDateRange() {
        return dateRange;
    }

    public void setDateRange(DateRange dateRange) {
        this.dateRange = dateRange;
    }

    public Reservation copy() {
        return new Reservation(id, guestCount, price, additionalServices, status, eligibleRooms, assignedRoom, roomType, guest, dateRange);
    }

    public String whatCheck() {
        if (dateRange.getStart().equals(LocalDate.now()))
            return "početak";
        else if (dateRange.getEnd().equals(LocalDate.now()))
            return "kraj";
        else
            return "";
    }
}
