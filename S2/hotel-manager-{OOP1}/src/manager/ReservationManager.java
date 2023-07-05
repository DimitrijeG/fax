package manager;

import database.DataBase;
import entity.Reservation;
import entity.user.User;
import main.Settings;
import manager.generic.NumericManager;
import type.DateRange;
import type.enumeration.ReservationStatus;
import util.InternalUtil;

import java.io.IOException;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Comparator;
import java.util.HashSet;
import java.util.TreeSet;

public class ReservationManager extends NumericManager {

    public ReservationManager() {
        super();
    }

    public ReservationManager(String filepath) throws IOException {
        super(filepath);
    }

    @Override
    protected void updateIdLength(Integer nextId) {
        // ne bi trebalo da se desi u realnim uslovima
        Settings.reservationIdLength = Math.max(String.valueOf(nextId).length(), Settings.reservationIdLength);
    }

    public Reservation getReservation(int id) {
        return (Reservation) get(id);
    }

    public boolean addReservation(Reservation reservation) {
        Integer newId = reservation.getId();
        if (newId == 0) { // ceka se da sistem dodeli Id
            newId = getNextId();
            reservation.setId(newId);
        }
        return add(newId, reservation);
    }

    public boolean updateReservation(Integer id, Reservation reservation) {
        remove(id);
        return add(reservation.getId(), reservation);
    }

    public boolean removeReservation(int id) {
        return remove(id);
    }

    @Override
    public void fetchData(String filepath) throws IOException {
        HashSet<Integer> validTokenLength = new HashSet<>();
        validTokenLength.add(10);
        ArrayList<String[]> allTokens = DataBase.readTokens(filepath, validTokenLength);

        Reservation newReservation;
        for (String[] tokens : allTokens) {
            int id = InternalUtil.parseInt(tokens[0]);
            int guestCount = InternalUtil.parseInt(tokens[1]);
            double price = InternalUtil.parseDouble(tokens[2]);
            TreeSet<Integer> additionalServices = InternalUtil.parseIntegerSet(tokens[3], Settings.delimiterB);
            ReservationStatus status = ReservationStatus.valueOf(tokens[4]);
            TreeSet<Integer> eligibleRooms = InternalUtil.parseIntegerSet(tokens[5], Settings.delimiterB);
            int assignedRoom = InternalUtil.parseInt(tokens[6]);
            int roomType = InternalUtil.parseInt(tokens[7]);
            String guest = tokens[8];

            String[] rangeTokens = tokens[9].split(Settings.delimiterB, -1);
            LocalDate start = InternalUtil.parseDate(rangeTokens[0].trim());
            LocalDate end = InternalUtil.parseDate(rangeTokens[1].trim());
            DateRange dateRange = new DateRange(start, end);

            newReservation = new Reservation(id, guestCount, price, additionalServices, status, eligibleRooms, assignedRoom, roomType, guest, dateRange);
            add(id, newReservation);
        }
    }

    @Override
    public void saveData(String filepath) throws IOException {
        ArrayList<String[]> allTokens = new ArrayList<>();

        sort(Comparator.comparing(o -> ((Reservation) o).getId()));
        for (Object value : values()) {
            Reservation reservation = (Reservation) value;
            DateRange dateRange = reservation.getDateRange();
            String[] tokens = new String[]{
                    InternalUtil.toString(reservation.getId()),
                    InternalUtil.toString(reservation.getGuestCount()),
                    InternalUtil.toString(reservation.getPrice()),
                    InternalUtil.toString(reservation.getAdditionalServices(), Settings.delimiterB),
                    InternalUtil.toString(reservation.getStatus()),
                    InternalUtil.toString(reservation.getEligibleRooms(), Settings.delimiterB),
                    InternalUtil.toString(reservation.getAssignedRoom()),
                    InternalUtil.toString(reservation.getRoomType()),
                    reservation.getGuest(),
                    InternalUtil.toString(dateRange.getStart()) +
                            Settings.delimiterB + InternalUtil.toString(dateRange.getEnd())
            };
            allTokens.add(tokens);
        }
        DataBase.writeTokens(filepath, allTokens);
    }

    public ArrayList<Object> pendingReservations() {
        ArrayList<Object> filteredReservations = new ArrayList<>();
        for (Object obj : values()) {
            Reservation r = (Reservation) obj;
            if (r.getStatus().equals(ReservationStatus.PENDING))
                filteredReservations.add(r);
        }
        return filteredReservations;
    }

    public ArrayList<Object> todaysReservations() {
        ArrayList<Object> filteredReservations = new ArrayList<>();
        for (Object obj : values()) {
            Reservation r = (Reservation) obj;
            LocalDate start = r.getDateRange().getStart();
            LocalDate end = r.getDateRange().getEnd();
            if (start.equals(LocalDate.now()) || end.equals(LocalDate.now()))
                filteredReservations.add(r);
        }
        return filteredReservations;
    }
}
