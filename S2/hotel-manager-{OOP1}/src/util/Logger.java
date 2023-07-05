package util;

import database.DataBase;
import type.enumeration.ReservationStatus;

import java.io.File;
import java.io.IOException;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;

public class Logger {
    String reservationLogs, roomLogs;

    public Logger(String reservationLogs, String roomLogs) throws IOException {
        if (!(new File(reservationLogs)).exists() || !(new File(roomLogs)).exists())
            throw new IOException();
        this.reservationLogs = reservationLogs;
        this.roomLogs = roomLogs;
    }

    public HashMap<String, ArrayList<LocalDate>> getRoomLogs() {
        HashMap<String, ArrayList<LocalDate>> logs = new HashMap<>();
        try {
            HashSet<Integer> validTokenLength = new HashSet<>();
            validTokenLength.add(2);
            ArrayList<String[]> lines = DataBase.readTokens(roomLogs, validTokenLength);
            for (String[] tokens : lines) {
                if (tokens.length == 0)
                    continue;
                String maid = tokens[0];
                LocalDate date = InternalUtil.parseDate(tokens[1]);
                if (!logs.containsKey(maid)) {
                    logs.put(maid, new ArrayList<>());
                }
                logs.get(maid).add(date);
            }
        } catch (IOException ignored) {
        }
        return logs;
    }
//
//    public void logCreatedReservation() {
//        logReservationStatus(ReservationStatus.PENDING);
//    }
//
//    public void logApprovedReservation() {
//        logReservationStatus(ReservationStatus.APPROVED);
//    }
//
//    public void logDeclinedReservation() {
//        logReservationStatus(ReservationStatus.DECLINED);
//    }
//
//    public void logWithdrewReservation() {
//        logReservationStatus(ReservationStatus.WITHDREW);
//    }

    public void logRoomCleanup(String maid) {
        ArrayList<String[]> lines = new ArrayList<>();
        lines.add(new String[]{
                maid,
                InternalUtil.toString(LocalDate.now())
        });
        try {
            DataBase.appendTokens(roomLogs, lines);
        } catch (IOException ignored) {
        }
    }
//
//    private void logReservationStatus(ReservationStatus status) {
//        ArrayList<String[]> lines = new ArrayList<>();
//        lines.add(new String[]{
//                InternalUtil.toString(status),
//                InternalUtil.toString(LocalDate.now())
//        });
//        try {
//            DataBase.appendTokens(reservationLogs, lines);
//        } catch (IOException ignored) {
//        }
//    }
//
//    private ArrayList<LocalDate> getReservationsWithStatus(ReservationStatus status) {
//        ArrayList<LocalDate> logs = new ArrayList<>();
//        try {
//            ArrayList<String[]> lines = DataBase.readTokens(reservationLogs);
//            for (String[] tokens : lines) {
//                if (tokens[0].equals(InternalUtil.toString(status)))
//                    logs.add(InternalUtil.parseDate(tokens[1]));
//            }
//        } catch (IOException ignored) {
//        }
//        return logs;
//    }
}
