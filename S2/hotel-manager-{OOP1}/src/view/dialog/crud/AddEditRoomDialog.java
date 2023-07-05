package view.dialog.crud;

import entity.Room;
import main.Settings;
import manager.ArticleMap;
import manager.RoomManager;
import net.miginfocom.swing.MigLayout;
import type.ExistingIdException;
import type.ValidatorException;
import type.enumeration.RoomStatus;
import util.Validate;
import view.dialog.MessageDialog;
import view.generic.InputForm;
import view.panel.FlowPanel;

import javax.swing.*;
import java.awt.*;
import java.util.ArrayList;
import java.util.TreeSet;

import static util.ViewUtil.*;

public class AddEditRoomDialog extends JDialog implements InputForm {
    private final RoomManager roomManager;
    private final Room initData;
    private Integer nextId = 0;
    private final ArticleMap articles;
    private final JTextField idField = new JTextField();
    private final JComboBox<String> typeCombo = new JComboBox<>(), statusCombo = new JComboBox<>();
    private final ArrayList<JCheckBox> amenityBoxes = new ArrayList<>();


    public AddEditRoomDialog(JFrame parent, RoomManager roomManager, Room room, ArticleMap articles) {
        super(parent, true);
        this.roomManager = roomManager;
        this.initData = room;
        this.articles = articles;

        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        setLocationRelativeTo(null);
        initialize();
    }

    @Override
    public void initialize() {
        LayoutManager dialogLayout = new MigLayout(
                "wrap 2,insets 15 30 15 30",
                "[::,right]10[::,left]",
                "[]8[]8[]8[::]12[]");
        setLayout(dialogLayout);

        for (String title : articles.getRoomTypes())
            typeCombo.addItem(title);

        statusCombo.addItem(RoomStatus.TAKEN.toString());
        statusCombo.addItem(RoomStatus.CLEANUP.toString());
        statusCombo.addItem(RoomStatus.FREE.toString());

        for (String title : articles.getAmenities())
            amenityBoxes.add(new JCheckBox(title));

//        ------------------------------------------------------------
        JPanel amenityPanel = FlowPanel.getPanel();
        for (JCheckBox cb : amenityBoxes)
            amenityPanel.add(cb);

        add(new Label("Id:"));
        add(idField, "growx");
        add(new Label("Tip:"));
        add(typeCombo, "growx");
        add(new Label("Status:"));
        add(statusCombo, "growx");
        add(new Label("Dodatna oprema:"));
        add(amenityPanel, "growx,growy");

        add(failStatus, "growx,span 2,align center");

        JButton submitButton;
        if (initData == null) {
            nextId = roomManager.getNextId();
            idField.setText(nextId.toString());
            setTitle("Dodavanje sobe");
            submitButton = initAdd("Dodaj");
        } else {
            setTitle("Izmena sobe");
            submitButton = initEdit("Izmeni");
        }
        add(submitButton);
        add(cancelButton);

        changeFont(this, Settings.font);
        pack();
        setVisible(true);
    }

    @Override
    public void validateFields() throws ValidatorException {
        Integer id = parseInteger(getText(idField));
        Integer suggestedId = nextId;
        if (initData == null || !id.equals(initData.getId())) {
            try {
                Validate.newId(getText(idField), roomManager.keys());
            } catch (ExistingIdException e) {
                if (initData != null)
                    suggestedId = initData.getId();
                if (MessageDialog.suggestId(this, suggestedId, e.getMessage()))
                    idField.setText(suggestedId.toString());
            }
        }
    }

    @Override
    public void addObject() {
        Room r = (Room) collectData();
        roomManager.addRoom(r);
    }

    @Override
    public void updateObject() {
        Room r = (Room) collectData();
        roomManager.updateRoom(initData.getId(), r);
    }

    @Override
    public Object collectData() {
        Integer id = parseInteger(getText(idField));
        String strType = (String) typeCombo.getSelectedItem();
        Integer type = articles.getRoomType(strType);
        int statusIndex = statusCombo.getSelectedIndex();
        RoomStatus status = RoomStatus.FREE;
        switch (statusIndex) {
            case 0:
                status = RoomStatus.TAKEN;
                break;
            case 1:
                status = RoomStatus.CLEANUP;
        }
        TreeSet<Integer> amenities = new TreeSet<>();

        for (JCheckBox cb : amenityBoxes)
            if (cb.isSelected())
                amenities.add(articles.getAmenity(cb.getText()));

        return new Room(id, type, status, amenities);
    }

    @Override
    public void populate() {
        idField.setText(initData.getId().toString());
        typeCombo.setSelectedItem(articles.getRoomType(initData.getType()));
        statusCombo.setSelectedItem(initData.getStatus().toString());
        for (JCheckBox cb : amenityBoxes) {
            if (initData.getAmenities().contains(articles.getAmenity(cb.getText())))
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
