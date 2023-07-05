package view.frame;

import entity.user.User;
import main.Settings;
import manager.CoreManager;
import util.RoomFilter;
import view.dialog.CreateReservationDialog;
import view.dialog.MessageDialog;
import view.frame.table.GuestReservationTableFrame;
import view.frame.table.GuestSearchTableFrame;
import view.generic.ExitMenu;
import view.generic.Window;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import static util.ViewUtil.changeFont;

public class GuestFrame extends JFrame implements Window {
    CoreManager app;

    public GuestFrame(CoreManager app) {
        super();
        this.app = app;
        initialize();
    }

    @Override
    public void initialize() {
        setTitle("Gost");
        setDefaultCloseOperation(JDialog.EXIT_ON_CLOSE);
        setBackground(Color.lightGray);
        setPreferredSize(new Dimension(900, 330));

        initMenu();

        pack();
        setLocationRelativeTo(null);
        changeFont(this, Settings.font);
        setVisible(true);
    }

    public void initMenu() {
        JMenuBar menuBar = new JMenuBar();
        JMenuItem roomTypeSearch = new JMenuItem("Pretraga tipova soba");
        JMenuItem makeReservationItem = new JMenuItem("Napravi rezervaciju");
        JMenuItem reservationsItem = new JMenuItem("Pregled rezervacija");

        menuBar.add(roomTypeSearch);
        menuBar.add(makeReservationItem);
        menuBar.add(reservationsItem);
        menuBar.add(new ExitMenu(this, app));
        changeFont(menuBar, Settings.font);
        add(menuBar, BorderLayout.NORTH);


        roomTypeSearch.addActionListener(e -> new GuestSearchTableFrame(new RoomFilter(app)));
        makeReservationItem.addActionListener(e -> {
            new CreateReservationDialog(this, app);
        });
        reservationsItem.addActionListener(e -> {
            if (app.guestReservations().size() != 0)
                new GuestReservationTableFrame(app);
            else
                MessageDialog.ok(null, "Trenutno nemate rezervacija!", "Obaveštenje", JOptionPane.INFORMATION_MESSAGE);
        });

        User user = app.getUser();
        JLabel welcomeLabel = new JLabel("Dobrodošli " + user.getName() + " " + user.getSurname() + ".");
        welcomeLabel.setHorizontalAlignment(JLabel.CENTER);
        welcomeLabel.setVerticalAlignment(JLabel.CENTER);
        add(welcomeLabel);
    }
}
