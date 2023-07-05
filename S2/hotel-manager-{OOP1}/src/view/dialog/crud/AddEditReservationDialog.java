package view.dialog.crud;

import entity.Reservation;
import main.Settings;
import manager.CoreManager;
import net.miginfocom.swing.MigLayout;
import type.DateException;
import type.DateRange;
import type.ExistingIdException;
import type.ValidatorException;
import type.enumeration.ReservationStatus;
import util.Validate;
import util.ViewUtil;
import view.dialog.MessageDialog;
import view.generic.InputForm;
import view.panel.FlowPanel;

import javax.swing.*;
import java.awt.*;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Objects;
import java.util.TreeSet;

import static util.ViewUtil.*;

public class AddEditReservationDialog extends JDialog implements InputForm {
    private final CoreManager app;
    private final Reservation initData;
    private Integer nextId = 0;
    private final JTextField idField = new JTextField(), priceField = new JTextField();
    private final JTextField guestField = new JTextField(), guestNumField = new JTextField();
    private final JTextField startField = new JTextField(), endField = new JTextField();
    private final JComboBox<String> statusCombo = new JComboBox<>(), roomTypeCombo = new JComboBox<>(), assignedRoomCombo = new JComboBox<>();
    private final JTextArea eligibleRoomsArea = new JTextArea();
    private final ArrayList<JCheckBox> addServiceBoxes = new ArrayList<>();

    public AddEditReservationDialog(JFrame parent, CoreManager app, Reservation r) {
        super(parent, true);
        this.app = app;
        this.initData = r;

        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        setLocationRelativeTo(null);
        initialize();
    }

    @Override
    public void initialize() {
        LayoutManager dialogLayout = new MigLayout(
                "wrap 2,insets 15 30 15 30",
                "[::,right]10[::,left]",
                "[]8[]8[]8[]8[]8[]8[]8[]8[]8[]12[]");
        setLayout(dialogLayout);

        statusCombo.addItem(ReservationStatus.PENDING.toString());
        statusCombo.addItem(ReservationStatus.APPROVED.toString());
        statusCombo.addItem(ReservationStatus.DECLINED.toString());
        statusCombo.addItem(ReservationStatus.WITHDREW.toString());

        for (String title : app.roomTypeManager.getTitles().values())
            roomTypeCombo.addItem(title);

//        ------------------------------------------------------------
        for (String title : app.addServiceManager.getTitles().values())
            addServiceBoxes.add(new JCheckBox(title));
        JPanel addServicesPanel = FlowPanel.getPanel();
        for (JCheckBox cb : addServiceBoxes)
            addServicesPanel.add(cb);

        priceField.setEditable(false);
        eligibleRoomsArea.setEditable(false);

        add(new Label("Id:"));
        add(idField, "growx");
        add(new Label("Status:"));
        add(statusCombo, "growx");
        add(new Label("Cena (RSD):"));
        add(priceField, "growx");
        add(new Label("Gost:"));
        add(guestField, "growx");
        add(new Label("Broj gostiju:"));
        add(guestNumField, "growx");
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
        add(new Label("Pogodne sobe:"));
        add(eligibleRoomsArea, "growx");
        add(new Label("Dodeljena soba:"));
        add(assignedRoomCombo, "growx");
        add(new Label("Dodatne usluge:"));
        add(addServicesPanel, "growx,growy");

        add(failStatus, "growx,span 2,align center");

        JButton submitButton;
        if (initData == null) {
            nextId = app.reservationManager.getNextId();
            idField.setText(nextId.toString());
            assignedRoomCombo.setEditable(false);
            setTitle("Dodavanje rezervacije");
            submitButton = initAdd("Dodaj");
        } else {
            idField.setText(initData.getId().toString());
            for (Integer id : initData.getEligibleRooms())
                assignedRoomCombo.addItem(id.toString());
            assignedRoomCombo.addItem("");
            assignedRoomCombo.setSelectedItem("");
            setTitle("Izmena rezervacije");
            submitButton = initEdit("Izmeni");
        }
        add(submitButton);
        add(cancelButton);

        changeFont(this, Settings.font);
        pack();
        setVisible(true);
    }

    private void validateDates() throws ValidatorException {
        String startTxt = getText(startField), endTxt = getText(endField);
        Validate.localDate(startTxt);
        Validate.localDate(endTxt);
        LocalDate start = ViewUtil.parseDate(startTxt);
        LocalDate end = ViewUtil.parseDate(endTxt);
        if (!start.isBefore(end)) throw new DateException("Početak rezervacije mora biti pre kraja.");
    }

    @Override
    public void validateFields() throws ValidatorException {
        Validate.existingUsername(getText(guestField), app.userManager.keys(), "Izabrani gost nije u sistemu!");
        Validate.id(getText(idField));
        Integer id = parseInteger(getText(idField));
        Integer suggestedId = nextId;
        if (initData == null || !id.equals(initData.getId())) {
            try {
                Validate.newId(getText(idField), app.reservationManager.keys());
            } catch (ExistingIdException e) {
                if (initData != null)
                    suggestedId = initData.getId();
                if (MessageDialog.suggestId(this, suggestedId, e.getMessage()))
                    idField.setText(suggestedId.toString());
            }
        }
        Validate.notEmpty(getText(guestField));
        Validate.integer(getText(guestNumField));
        validateDates();
    }

    @Override
    public void addObject() {
        Reservation r = (Reservation) collectData();
        app.reservationManager.addReservation(r);
    }

    @Override
    public void updateObject() {
        Reservation r = (Reservation) collectData();
        app.reservationManager.updateReservation(initData.getId(), r);
    }

    @Override
    public Object collectData() {
        Integer id = parseInteger(getText(idField));
        int statusIndex = statusCombo.getSelectedIndex();
        ReservationStatus status = ReservationStatus.WITHDREW;
        switch (statusIndex) {
            case 0:
                status = ReservationStatus.PENDING;
                break;
            case 1:
                status = ReservationStatus.APPROVED;
                break;
            case 2:
                status = ReservationStatus.DECLINED;
        }
        double price = ViewUtil.parseDouble(getText(priceField));
        String guest = getText(guestField);
        Integer guestNum = ViewUtil.parseInteger(getText(guestNumField));
        LocalDate start = ViewUtil.parseDate(getText(startField));
        LocalDate end = ViewUtil.parseDate(getText(endField));
        String strRoomType = (String) roomTypeCombo.getSelectedItem();
        Integer roomType = app.getArticleMap().getRoomType(strRoomType);
        TreeSet<Integer> eligibleRooms = new TreeSet<>();
        int assignedRoom = 0;
        if (initData != null && initData.getEligibleRooms().size() != 0) {
            eligibleRooms = initData.getEligibleRooms();
            if (!Objects.equals(assignedRoomCombo.getSelectedItem(), ""))
                assignedRoom = ViewUtil.parseInteger((String) assignedRoomCombo.getSelectedItem());
        }
        TreeSet<Integer> addServices = new TreeSet<>();
        for (JCheckBox cb : addServiceBoxes)
            if (cb.isSelected())
                addServices.add(app.getArticleMap().getAddService(cb.getText()));

        return new Reservation(id, guestNum, price, addServices, status, eligibleRooms, assignedRoom, roomType, guest, new DateRange(start, end));
    }

    @Override
    public void populate() {
        idField.setText(initData.getId().toString());
        statusCombo.setSelectedItem(initData.getStatus().toString());
        priceField.setText(ViewUtil.toString(initData.getPrice()));
        guestField.setText(initData.getGuest());
        guestNumField.setText(initData.getGuestCount().toString());
        startField.setText(ViewUtil.toString(initData.getDateRange().getStart()));
        endField.setText(ViewUtil.toString(initData.getDateRange().getEnd()));
        roomTypeCombo.setSelectedItem(app.getArticleMap().getRoomType(initData.getRoomType()));
        String[] eligibleRooms = ViewUtil.toIdArray(initData.getEligibleRooms(), Settings.articleIdLength);
        eligibleRoomsArea.setText(String.join("\n", eligibleRooms));
        for (JCheckBox cb : addServiceBoxes) {
            if (initData.getAdditionalServices().contains(app.getArticleMap().getAddService(cb.getText())))
                cb.setSelected(true);
        }
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
