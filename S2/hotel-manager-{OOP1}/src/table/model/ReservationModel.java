package table.model;

import entity.Reservation;
import main.Settings;
import manager.CoreManager;
import util.ViewUtil;

import java.util.ArrayList;

public class ReservationModel extends TableModel {

    CoreManager app;

    public ReservationModel(CoreManager app) {
        super(new String[]{
                "Id<br>&nbsp", "Status", "Cena", "Gost", "Broj<br>gostiju",
                "Trajanje", "Tip sobe", "Pogodne<br>sobe",
                "Dodeljena<br>soba", "Dodatne<br>usluge"
        });
        this.app = app;
    }

    @Override
    protected ArrayList<Object> getData() {
        return app.reservationManager.values();
    }

    @Override
    public Object getValueAt(int rowIndex, int columnIndex) {
        Reservation reservation = (Reservation) app.reservationManager.values().get(rowIndex);
        switch (columnIndex) {
            case 0:
                return ViewUtil.formatId(reservation.getId(), Settings.reservationIdLength);
            case 1:
                return reservation.getStatus();
            case 2:
                return reservation.getPrice();
            case 3:
                return reservation.getGuest();
            case 4:
                return reservation.getGuestCount();
            case 5:
                return reservation.getDateRange();
            case 6:
                return app.getArticleMap().getRoomType(reservation.getRoomType());
            case 7:
                return ViewUtil.toIdArray(
                        reservation.getEligibleRooms(),
                        Settings.roomIdLength);
            case 8:
                return ViewUtil.formatId(reservation.getAssignedRoom(), Settings.roomIdLength);
            case 9:
                return ViewUtil.toStringArray(
                        reservation.getAdditionalServices(),
                        app.getArticleMap().addServices);
            default:
                return null;
        }

    }
}
