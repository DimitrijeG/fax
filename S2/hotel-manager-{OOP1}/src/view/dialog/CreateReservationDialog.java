package view.dialog;

import entity.Reservation;
import entity.Room;
import main.Settings;
import manager.CoreManager;
import net.miginfocom.swing.MigLayout;
import sun.reflect.generics.tree.Tree;
import type.DateException;
import type.DateRange;
import type.ValidatorException;
import type.enumeration.ReservationStatus;
import util.RoomFilter;
import util.Validate;
import util.ViewUtil;
import view.generic.InputForm;
import view.panel.FlowPanel;

import javax.swing.*;
import java.awt.*;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.TreeSet;

import static util.ViewUtil.*;

public class CreateReservationDialog extends JDialog implements InputForm {
    private final CoreManager app;
    private final JTextField idField = new JTextField(), priceField = new JTextField();
    private final JTextField guestField = new JTextField(), guestNumField = new JTextField();
    private final JTextField startField = new JTextField(), endField = new JTextField();
    private final JComboBox<String> roomTypeCombo = new JComboBox<>();
    private final ArrayList<JCheckBox> addServiceBoxes = new ArrayList<>();
    private final TreeSet<Integer> amenities = new TreeSet<>();

    public CreateReservationDialog(JFrame parent, CoreManager app) {
        super(parent, true);
        this.app = app;

        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        setLocationRelativeTo(null);
        for (String title : app.roomTypeManager.getTitles().values())
            roomTypeCombo.addItem(title);
        initialize();
    }

    public CreateReservationDialog(JFrame parent, CoreManager app, Room room, DateRange range) {
        super(parent, true);
        this.app = app;

        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        setLocationRelativeTo(null);

        for (String title : app.roomTypeManager.getTitles().values())
            roomTypeCombo.addItem(title);
        startField.setText(ViewUtil.toString(range.getStart()));
        endField.setText(ViewUtil.toString(range.getEnd()));
        startField.setEditable(false);
        endField.setEditable(false);
        amenities.addAll(room.getAmenities());
        if (!room.getType().equals(0)) {
            roomTypeCombo.setSelectedItem(app.getArticleMap().getRoomType(room.getType()));
            String a = (String) roomTypeCombo.getSelectedItem();
            roomTypeCombo.setEditable(false);

            try {
                double price = app.calculatePrice(
                        room.getType(),
                        getSelectedAdditionalServices(),
                        getSelectedDateRange()
                );
                priceField.setText(ViewUtil.toString(price));
            } catch (DateException ex) {
                failStatus.setText(ex.getMessage());
            } catch (ValidatorException ignored) {
            }
        }
        initialize();
    }

    @Override
    public void initialize() {
        LayoutManager dialogLayout = new MigLayout(
                "wrap 2,insets 15 30 15 30",
                "[::,right]10[::,left]",
                "[]8[]8[]8[]8[]8[]8[]12[]");
        setLayout(dialogLayout);

        roomTypeCombo.addActionListener(e -> {
            try {
                double price = app.calculatePrice(
                        getSelectedRoomType(),
                        getSelectedAdditionalServices(),
                        getSelectedDateRange()
                );
                priceField.setText(ViewUtil.toString(price));
            } catch (DateException ex) {
                failStatus.setText(ex.getMessage());
            } catch (ValidatorException ignored) {
            }
        });

//        ------------------------------------------------------------
        for (String title : app.addServiceManager.getTitles().values()) {
            JCheckBox addServiceCheckBox = new JCheckBox(title);
            addServiceCheckBox.addActionListener(e -> {
                try {
                    double price = app.calculatePrice(
                            getSelectedRoomType(),
                            getSelectedAdditionalServices(),
                            getSelectedDateRange()
                    );
                    priceField.setText(ViewUtil.toString(price));
                } catch (DateException ex) {
                    failStatus.setText(ex.getMessage());
                } catch (ValidatorException ignored) {
                }
            });
            addServiceBoxes.add(addServiceCheckBox);
        }
        JPanel addServicesPanel = FlowPanel.getPanel();
        for (JCheckBox cb : addServiceBoxes)
            addServicesPanel.add(cb);

        priceField.setEditable(false);
        guestField.setText(app.getUser().getUsername());

        add(new Label("Gost:"));
        add(guestField, "growx");
        add(new Label("Broj gostiju:"));
        add(guestNumField, "growx");
        add(new Label("Cena (RSD):"));
        add(priceField, "growx");
        JPanel durationPanel = FlowPanel.getPanel();
        durationPanel.add(new Label("početak: "));
        startField.setPreferredSize(new Dimension(200, 40));
        durationPanel.add(startField);
        durationPanel.add(new Label("kraj: "));
        endField.setPreferredSize(new Dimension(200, 40));
        durationPanel.add(endField);
        add(new Label("Datumi rezervacije:"));
        add(durationPanel, "growx");
        add(new Label("Tip sobe: "));
        add(roomTypeCombo, "growx");
        add(new Label("Dodatne usluge:"));
        add(addServicesPanel, "growx,growy");

        add(failStatus, "growx,span 2,align center");
        add(initAdd("Dodaj"));
        add(cancelButton);

        Integer nextId = app.reservationManager.getNextId();
        idField.setText(nextId.toString());
        setTitle("Kreiranje rezervacije");
        changeFont(this, Settings.font);
        pack();
        setVisible(true);
    }

    private TreeSet<Integer> getSelectedAdditionalServices() {
        TreeSet<Integer> addServices = new TreeSet<>();
        for (JCheckBox cb : addServiceBoxes)
            if (cb.isSelected())
                addServices.add(app.getArticleMap().getAddService(cb.getText()));
        return addServices;
    }

    private Integer getSelectedRoomType() {
        String strRoomType = (String) roomTypeCombo.getSelectedItem();
        return app.getArticleMap().getRoomType(strRoomType);
    }

    private DateRange getSelectedDateRange() throws ValidatorException {
        validateDates();
        LocalDate start = ViewUtil.parseDate(getText(startField));
        LocalDate end = ViewUtil.parseDate(getText(endField));
        return new DateRange(start, end);
    }

    private void validateDates() throws ValidatorException {
        String startTxt = getText(startField), endTxt = getText(endField);
        Validate.localDate(startTxt);
        Validate.localDate(endTxt);
        LocalDate start = ViewUtil.parseDate(startTxt);
        LocalDate end = ViewUtil.parseDate(endTxt);
        if (!start.isBefore(end)) throw new DateException("Početak rezervacije mora biti pre kraja.");
        if (!start.isAfter(LocalDate.now())) throw new DateException("Početak rezervacije može biti najranije sutra.");
    }

    @Override
    public void validateFields() throws ValidatorException {
        Validate.existingUsername(getText(guestField), app.userManager.keys(), "Izabrani gost nije u sistemu!");
        validateDates();
        Validate.newId(getText(idField), app.reservationManager.keys());
        Validate.integer(getText(guestNumField));
        RoomFilter filter = new RoomFilter(app);
        DateRange range;
        try {
            range = getSelectedDateRange();
        } catch (ValidatorException e) {
            return;
        }
        Integer roomType = getSelectedRoomType();
        filter.reduceRooms(roomType, range, amenities);
        if (filter.getRooms().size() == 0) throw new ValidatorException("Nema slobodnih soba za unete kriterijume.");
    }

    @Override
    public void addObject() {
        Reservation r = (Reservation) collectData();
        app.reservationManager.addReservation(r);
    }

    @Override
    public void updateObject() {
    }

    @Override
    public void populate() {
    }

    @Override
    public Object collectData() {
        Integer id = parseInteger(getText(idField));
        ReservationStatus status = ReservationStatus.PENDING;
        String guest = getText(guestField);
        Integer guestNum = ViewUtil.parseInteger(getText(guestNumField));
        DateRange range;
        try {
            range = getSelectedDateRange();
        } catch (ValidatorException e) {
            return null;
        }
        Integer roomType = getSelectedRoomType();
        RoomFilter filter = new RoomFilter(app);
        filter.reduceRooms(roomType, range, amenities);
        TreeSet<Integer> eligibleRooms = filter.getRooms();
        int assignedRoom = 0;
        TreeSet<Integer> addServices = getSelectedAdditionalServices();
        double price = app.calculatePrice(getSelectedRoomType(), getSelectedAdditionalServices(), range);

        return new Reservation(id, guestNum, price, addServices, status, eligibleRooms, assignedRoom, roomType, guest, range);
    }

    @Override
    public boolean confirmEdit() {
        return MessageDialog.confirm(this, "Potvrđujete unesene izmene?");
    }

    @Override
    public void close() {
        dispose();
    }
}
