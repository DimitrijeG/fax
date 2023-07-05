package view.dialog;

import entity.Reservation;
import entity.Room;
import main.Settings;
import manager.CoreManager;
import net.miginfocom.swing.MigLayout;
import type.enumeration.RoomStatus;
import util.ViewUtil;

import javax.swing.*;
import java.awt.*;

import static util.ViewUtil.changeFont;

public class CheckInDialog extends JDialog {
    private final CoreManager app;
    private final Reservation initData;
    private final JButton checkInButton = new JButton("Check in");
    private final JComboBox<String> roomCombo = new JComboBox<>();


    public CheckInDialog(JFrame parent, CoreManager app, Reservation r) {
        super(parent, true);
        this.app = app;
        this.initData = r;

        initialize();
        initActions();

        setTitle("Check in");
        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        pack();
        setLocationRelativeTo(null);
        setVisible(true);
    }

    public void initialize() {
        setLayout(new MigLayout(
                "wrap 1,insets 30 50 20 50",
                "[::150,center]",
                "[]8[]20[]"
        ));

        for (Integer room : initData.getEligibleRooms()) {
            roomCombo.addItem(ViewUtil.formatId(room, Settings.roomIdLength));
        }
        add(new Label("Dodelite sobu rezervaciji: "), "growx");
        add(roomCombo, "growx");
        add(checkInButton);

        changeFont(this, Settings.font);
    }

    public void initActions() {
        checkInButton.addActionListener(e -> {
            int roomId = ViewUtil.parseInteger((String) roomCombo.getSelectedItem());
            initData.setAssignedRoom(roomId);
            Room r = app.roomManager.getRoom(roomId);
            r.setStatus(RoomStatus.TAKEN);
            dispose();
        });
    }
}
