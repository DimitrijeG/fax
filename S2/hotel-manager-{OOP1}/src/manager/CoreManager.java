package manager;

import database.DataBase;
import entity.Reservation;
import entity.Room;
import entity.user.Employee;
import entity.user.User;
import type.DateRange;
import type.enumeration.EmployeeRole;
import type.enumeration.ReservationStatus;
import type.enumeration.RoomStatus;
import util.Logger;

import java.io.IOException;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.TreeSet;

public class CoreManager {
    public UserManager userManager;
    public RoomManager roomManager;
    public ReservationManager reservationManager;
    public ArticleManager amenityManager, addServiceManager, roomTypeManager;
    public Logger logger;
    private LinkedHashMap<String, TreeSet<Integer>> assignedRooms;
    private User currentUser;

    public CoreManager(String[] f) throws IOException {
        userManager = new UserManager(f[0]);
        roomManager = new RoomManager(f[1]);
        reservationManager = new ReservationManager(f[2]);
        amenityManager = new ArticleManager(f[3]);
        addServiceManager = new ArticleManager(f[4]);
        roomTypeManager = new ArticleManager(f[5]);
        logger = new Logger(f[6], f[7]);
        currentUser = null;
        assignRooms();
        updateReservationStatuses();
    }

    public CoreManager(String dirPath) throws IOException {
        this(DataBase.getFiles(dirPath));
    }

    public CoreManager() throws IOException {
        this("data");
    }

    public User getUser() {
        return currentUser;
    }

    public void setUser(String username) {
        if (username.isEmpty())
            currentUser = null;
        currentUser = userManager.getUser(username);
    }

    public void setUser() {
        setUser("");
    }

    public ArticleMap getArticleMap() {
        return new ArticleMap(amenityManager, roomTypeManager, addServiceManager);
    }

    private void assignRooms() {
        assignedRooms = new LinkedHashMap<>();

        String maid = null;
        int mostOptimal;
        for (Object room : roomManager.values()) {
            mostOptimal = -1;
            if (!((Room) room).getStatus().equals(RoomStatus.CLEANUP))
                continue;
            for (Object obj : userManager.employees()) {
                Employee employee = (Employee) obj;
                if (employee.getRole().equals(EmployeeRole.MAID)) {
                    String username = employee.getUsername();
                    if (!assignedRooms.containsKey(username))
                        assignedRooms.put(username, new TreeSet<>());
                    if (mostOptimal == -1 || assignedRooms.get(username).size() <= mostOptimal) {
                        maid = username;
                        mostOptimal = assignedRooms.get(maid).size();
                    }
                }
            }
            if (maid == null)
                return;
            assignedRooms.get(maid).add(((Room) room).getId());
        }
    }

    public ArrayList<Object> getAssignedRooms(String maid) {
        ArrayList<Object> rooms = new ArrayList<>();
        for (Integer id : assignedRooms.get(maid)) {
            rooms.add(roomManager.getRoom(id));
        }
        return rooms;
    }

    public void assignRoom(Room r) {
        String maid = null;
        int mostOptimal = -1;
        if (!r.getStatus().equals(RoomStatus.CLEANUP))
            return;
        for (Object obj : userManager.employees()) {
            Employee employee = (Employee) obj;
            if (employee.getRole().equals(EmployeeRole.MAID)) {
                String username = employee.getUsername();
                if (!assignedRooms.containsKey(username))
                    assignedRooms.put(username, new TreeSet<>());
                if (mostOptimal == -1 || assignedRooms.get(username).size() <= mostOptimal) {
                    maid = username;
                    mostOptimal = assignedRooms.get(maid).size();
                }
            }
        }
        if (maid == null)
            return;
        assignedRooms.get(maid).add(r.getId());
    }

    private void updateReservationStatuses() {
        for (Object obj : reservationManager.values()) {
            Reservation r = (Reservation) obj;
            if (r.getStatus().equals(ReservationStatus.PENDING) &&
                    !r.getDateRange().getStart().isAfter(LocalDate.now()))
                r.setStatus(ReservationStatus.WITHDREW);
        }
    }

    public ArrayList<Object> guestReservations() {
        ArrayList<Object> filteredReservations = new ArrayList<>();
        for (Object obj : reservationManager.values()) {
            Reservation r = (Reservation) obj;
            if (r.getGuest().equals(currentUser.getUsername()))
                filteredReservations.add(r);
        }
        return filteredReservations;
    }

    public double calculatePrice(Integer roomType, TreeSet<Integer> addServices, DateRange range) {
        LocalDate start = range.getStart();
        double roomTypePrice = roomTypeManager.getArticle(roomType).calculatePrice(range);
        double selectedServicePrice = 0;
        for (Integer addService : addServices) {
            selectedServicePrice += addServiceManager.getArticle(addService).calculatePrice(start);
        }
        return roomTypePrice + selectedServicePrice;
    }

    public void saveData(String[] f) throws IOException {
        userManager.saveData(f[0]);
        roomManager.saveData(f[1]);
        reservationManager.saveData(f[2]);
        amenityManager.saveData(f[3]);
        addServiceManager.saveData(f[4]);
        roomTypeManager.saveData(f[5]);
    }

    public void saveData(String dirPath) throws IOException {
        saveData(DataBase.getFiles(dirPath));
    }

    public void saveData() throws IOException {
        saveData("data");
    }
}
