package view.frame.table;

import entity.Room;
import main.Settings;
import manager.CoreManager;
import net.miginfocom.swing.MigLayout;
import table.model.GuestSearchModel;
import table.model.MaidRoomModel;
import type.DateException;
import type.DateRange;
import type.ValidatorException;
import type.enumeration.RoomStatus;
import util.Compare;
import util.RoomFilter;
import util.Validate;
import util.ViewUtil;
import view.dialog.CreateReservationDialog;
import view.dialog.MessageDialog;
import view.panel.FlowPanel;

import javax.swing.*;
import javax.swing.event.DocumentEvent;
import javax.swing.event.DocumentListener;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.TreeSet;

import static util.ViewUtil.changeFont;
import static util.ViewUtil.getText;

public class GuestSearchTableFrame extends TableFrame implements DocumentListener {
    private final RoomFilter roomFilter;
    private JTextField startField = new JTextField(), endField = new JTextField();
    private final JComboBox<String> roomTypeCombo = new JComboBox<>();
    private final ArrayList<JCheckBox> amenityBoxes = new ArrayList<>();
    private JButton buttonReserve, search;
    JLabel failStatus = new JLabel(" ");

    public GuestSearchTableFrame(RoomFilter roomFilter) {
        super(new GuestSearchModel(roomFilter));
        this.roomFilter = roomFilter;
        initSearchBar();
        setTitle("Pretraga soba");
        setSize(1000, 800);
    }

    private void initSearchBar() {
        failStatus.setForeground(Color.red);
        failStatus.setOpaque(true);
        failStatus.setText(" ");
        buttonReserve.setEnabled(false);


        for (String title : roomFilter.app.roomTypeManager.getTitles().values())
            roomTypeCombo.addItem(title);
        roomTypeCombo.addItem("");

        JPanel searchBar = new JPanel(new MigLayout(
                "wrap 2", "[right]10[left,grow]", "30[]30[]8[]8[]8[]"
        ));

        JPanel durationPanel = FlowPanel.getPanel();
        durationPanel.add(new Label("početak: "));
        startField.setPreferredSize(new Dimension(200, 40));
        durationPanel.add(startField);
        durationPanel.add(new Label("kraj: "));
        endField.setPreferredSize(new Dimension(200, 40));
        durationPanel.add(endField);

        searchBar.add(new Label("Datumi rezervacije:"));
        searchBar.add(durationPanel, "growx");

        searchBar.add(new Label("Tip sobe: "));
        searchBar.add(roomTypeCombo, "growx");

        for (String title : roomFilter.app.amenityManager.getTitles().values())
            amenityBoxes.add(new JCheckBox(title));
        JPanel amenityPanel = FlowPanel.getPanel();
        for (JCheckBox cb : amenityBoxes)
            amenityPanel.add(cb);

        searchBar.add(new Label("Dodatne usluge:"));
        searchBar.add(amenityPanel, "growx,growy");

        searchBar.add(failStatus, "span 2,align center");
        searchBar.add(search, "span 2,align center");

        changeFont(searchBar, Settings.font);
        add(searchBar, BorderLayout.SOUTH);

        startField.getDocument().addDocumentListener(this);
        endField.getDocument().addDocumentListener(this);
        roomTypeCombo.addActionListener(e -> disableReserve());
        for (JCheckBox cb : amenityBoxes)
            cb.addActionListener(e -> disableReserve());
    }

    @Override
    protected void initSpecificToolBar() {
        ImageIcon addIcon = new ImageIcon("img/add.png");
        addIcon = new ImageIcon(addIcon.getImage().getScaledInstance(20, 20, Image.SCALE_SMOOTH));

        buttonReserve = new JButton("Rezerviši");
        buttonReserve.setIcon(addIcon);
        search = new JButton("Pretraga");
        startField = new JTextField();
        endField = new JTextField();
        mainToolbar.add(buttonReserve, BorderLayout.NORTH);
    }

    @Override
    protected void initActions() {
        buttonReserve.addActionListener(e -> {
            int row = table.getSelectedRow();
            if (row == -1)
                MessageDialog.tableRowNotSelected();
            else {
                Integer roomType = roomFilter.app.getArticleMap().getRoomType(table.getValueAt(row, 0).toString());
                Room room = new Room(roomType, RoomStatus.FREE, getSelectedAmenities());
                new CreateReservationDialog(null, roomFilter.app, room, getSelectedDateRange());
            }
        });
        search.addActionListener(e -> {
            failStatus.setText(" ");
            try {
                validateDates();

                roomFilter.reset();
                DateRange range = getSelectedDateRange();
                Integer roomType = getSelectedRoomType();
                TreeSet<Integer> amenities = getSelectedAmenities();

                if (roomType == 0)
                    roomFilter.reduceRooms(range, amenities);
                else
                    roomFilter.reduceRooms(roomType, range, amenities);
                updateTable();
                buttonReserve.setEnabled(true);
            } catch (ValidatorException ex) {
                failStatus.setText(ex.getMessage());
            }
        });
    }

    public void disableReserve() {
        buttonReserve.setEnabled(false);
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

    private Integer getSelectedRoomType() {
        String strRoomType = (String) roomTypeCombo.getSelectedItem();
        assert strRoomType != null;
        if (strRoomType.isEmpty())
            return 0;
        return roomFilter.app.getArticleMap().getRoomType(strRoomType);
    }

    private DateRange getSelectedDateRange() {
        LocalDate start = ViewUtil.parseDate(getText(startField));
        LocalDate end = ViewUtil.parseDate(getText(endField));
        return new DateRange(start, end);
    }

    private TreeSet<Integer> getSelectedAmenities() {
        TreeSet<Integer> amenities = new TreeSet<>();
        for (JCheckBox cb : amenityBoxes)
            if (cb.isSelected())
                amenities.add(roomFilter.app.getArticleMap().getAmenity(cb.getText()));
        return amenities;
    }

    // Manuelni sorter - potrebno za razumevanje rada podrazumevanog sortera tabele
    @Override
    protected void sortData(int index) {
        roomFilter.app.roomManager.sort((o1, o2) -> {
            Room r1 = (Room) o1;
            Room r2 = (Room) o2;
            int retVal = 0;
            switch (index) {
                case 0:
                    retVal = r1.getType().compareTo(r2.getType());
                    break;
                case 1:
                    retVal = Compare.compare(r1.getAmenities(), r2.getAmenities());
                    break;
                default:
                    System.out.println("Too many columns to sort.");
                    System.exit(1);
            }
            return retVal * sortOrder.get(index);
        });
    }

    @Override
    public void insertUpdate(DocumentEvent e) {
        disableReserve();
    }

    @Override
    public void removeUpdate(DocumentEvent e) {
        disableReserve();
    }

    @Override
    public void changedUpdate(DocumentEvent e) {
        disableReserve();
    }
}
