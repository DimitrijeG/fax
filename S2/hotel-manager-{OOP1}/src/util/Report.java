package util;

import entity.Reservation;
import entity.user.Employee;
import manager.CoreManager;
import type.DateRange;
import type.enumeration.ReservationStatus;

import java.io.IOException;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

public class Report {
    CoreManager app;

    public Report() throws IOException {
        app = new CoreManager();
    }

    public Report(CoreManager app) {
        this.app = app;
    }

    public double getIncome(DateRange range) {
        double income = 0;
        for (Object obj : app.reservationManager.values()) {
            Reservation r = (Reservation) obj;
            if (range.contains(r.getDateRange()) && r.getDateRange().getEnd().isBefore(LocalDate.now()))
                income += r.getPrice();
        }
        return income;
    }

    public double getExpenditure(DateRange range) {
        double expenditure = 0;
        for (Object obj : app.reservationManager.values()) {
            Reservation r = (Reservation) obj;
            if (range.contains(r.getDateRange()) &&
                    r.getDateRange().getEnd().isBefore(LocalDate.now()) &&
                    r.getStatus().equals(ReservationStatus.DECLINED)
            )
                expenditure += r.getPrice();
        }

        if (range.getStart().equals(LocalDate.MIN))
            range.setStart(LocalDate.now().minusYears(1));
        if (range.getEnd().isAfter(LocalDate.now()))
            range.setEnd(LocalDate.now());

        for (LocalDate d : range.getDates())
            if (d.getDayOfMonth() == 1)
                expenditure += calculateSalaries();
        return expenditure;
    }

    private double calculateSalaries() {
        double salaries = 0;
        for (Object obj : app.userManager.employees())
            salaries += ((Employee) obj).getSalary();
        return salaries;
    }

    public Integer getProcessedReservations(DateRange range) {
        return countReservationsInRange(range, app.reservationManager.values());
    }

    public Integer getApprovedReservations(DateRange range) {
        return countReservationsInRange(range, getReservationsWithStatus(ReservationStatus.APPROVED));
    }

    public Integer getDeclinedReservations(DateRange range) {
        return countReservationsInRange(range, getReservationsWithStatus(ReservationStatus.DECLINED));
    }

    public Integer getWithdrewReservations(DateRange range) {
        return countReservationsInRange(range, getReservationsWithStatus(ReservationStatus.WITHDREW));
    }

    public ArrayList<Object> getReservationsWithStatus(ReservationStatus status) {
        ArrayList<Object> reservations = new ArrayList<>();
        for (Object obj : app.reservationManager.values())
            if (((Reservation) obj).getStatus().equals(status))
                reservations.add(obj);
        return reservations;
    }

    public Integer countReservationsInRange(DateRange range, ArrayList<Object> reservations) {
        int count = 0;
        for (Object obj : reservations)
            if (range.contains(((Reservation) obj).getDateRange()))
                count++;
        return count;
    }

    public ArrayList<Object[]> getMaidStats(DateRange range) {
        HashMap<String, ArrayList<LocalDate>> logs = app.logger.getRoomLogs();

        ArrayList<Object[]> statsCollection = new ArrayList<>();
        String username;
        ArrayList<LocalDate> dates;
        int count;
        for (Map.Entry<String, ArrayList<LocalDate>> entry : logs.entrySet()) {
            count = 0;
            username = entry.getKey();
            dates = entry.getValue();
            for (LocalDate d : dates)
                if (range.contains(d))
                    count++;
            statsCollection.add(new Object[]{
                    username,
                    app.userManager.getUser(username).getName(),
                    app.userManager.getUser(username).getSurname(),
                    count
            });
        }
        return statsCollection;
    }


    private static class RoomStats {
        public Integer id, roomType;
        public Integer nightNum;
        public double income;

        public RoomStats(Integer id, Integer roomType, int nightNum, double income) {
            this.id = id;
            this.roomType = roomType;
            this.nightNum = nightNum;
            this.income = income;
        }
    }

    public ArrayList<Object[]> getRoomStats(DateRange range) {
        ArrayList<Object[]> statsCollection = new ArrayList<>();

        HashMap<Integer, RoomStats> roomMap = new HashMap<>();
        double incomePerDay;
        int nightsNum;
        for (Object obj : app.reservationManager.values()) {
            Reservation r = (Reservation) obj;
            Integer roomId = r.getAssignedRoom();
            if (r.getStatus().equals(ReservationStatus.PENDING) ||
                    r.getStatus().equals(ReservationStatus.DECLINED) ||
                    roomId.equals(0))
                continue;

            ArrayList<LocalDate> dateList = r.getDateRange().getDates();
            nightsNum = 0;
            incomePerDay = r.getPrice() / dateList.size();
            for (LocalDate d : dateList)
                if (range.contains(d))
                    nightsNum++;

            if (roomMap.containsKey(roomId)) {
                RoomStats stat = roomMap.get(roomId);
                stat.nightNum += nightsNum;
                stat.income += incomePerDay * nightsNum;
            } else
                roomMap.put(roomId, new RoomStats(
                        roomId,
                        app.roomManager.getRoom(roomId).getType(),
                        nightsNum,
                        incomePerDay * nightsNum));
        }

        for (RoomStats stat : roomMap.values())
            statsCollection.add(new Object[]{stat.id, stat.roomType, stat.nightNum, stat.income});

        return statsCollection;
    }
}
