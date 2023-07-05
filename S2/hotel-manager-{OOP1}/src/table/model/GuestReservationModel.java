package table.model;

import entity.Reservation;
import main.Settings;
import manager.CoreManager;
import util.ViewUtil;

import java.util.ArrayList;

public class GuestReservationModel extends TableModel {

    CoreManager app;

    public GuestReservationModel(CoreManager app) {
        super(new String[]{
                "Id<br>&nbsp", "Status", "Cena", "Gost", "Broj<br>gostiju",
                "Trajanje", "Tip sobe", "Dodatne<br>usluge"
        });
        this.app = app;
    }

    @Override
    protected ArrayList<Object> getData() {
        return app.guestReservations();
    }

    @Override
    public Object getValueAt(int rowIndex, int columnIndex) {
        Reservation reservation = (Reservation) app.guestReservations().get(rowIndex);
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
                return ViewUtil.toStringArray(
                        reservation.getAdditionalServices(),
                        app.getArticleMap().addServices);
            default:
                return null;
        }

    }
}
