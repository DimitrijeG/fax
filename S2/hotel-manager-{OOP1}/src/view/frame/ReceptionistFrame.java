package view.frame;

import entity.user.User;
import main.Settings;
import manager.CoreManager;
import view.dialog.MessageDialog;
import view.dialog.crud.AddEditGuestDialog;
import view.frame.table.CheckInOutReservationiTableFrame;
import view.frame.table.PendingReservationTableFrame;
import view.generic.ExitMenu;
import view.generic.Window;

import javax.swing.*;
import java.awt.*;

import static util.ViewUtil.changeFont;

public class ReceptionistFrame extends JFrame implements Window {
    CoreManager app;

    public ReceptionistFrame(CoreManager app) {
        super();
        this.app = app;
        initialize();
    }

    @Override
    public void initialize() {
        setTitle("Recepcioner");
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
        JMenuItem pendingReservationsItem = new JMenuItem("Rezervacije na čekanju");
        JMenuItem checkInOutItem = new JMenuItem("Check in/out");
        JMenuItem createGuestItem = new JMenuItem("Dodaj gosta");

        menuBar.add(pendingReservationsItem);
        menuBar.add(checkInOutItem);
        menuBar.add(createGuestItem);
        menuBar.add(new ExitMenu(this, app));
        changeFont(menuBar, Settings.font);
        add(menuBar, BorderLayout.NORTH);

        pendingReservationsItem.addActionListener(e -> {
            if (app.reservationManager.pendingReservations().size() != 0)
                new PendingReservationTableFrame(app);
            else
                MessageDialog.ok(null, "Trenutno nema rezervacija na čekanju!", "Obaveštenje", JOptionPane.INFORMATION_MESSAGE);
        });
        checkInOutItem.addActionListener(e -> {
            if (app.reservationManager.todaysReservations().size() != 0)
                new CheckInOutReservationiTableFrame(app);
            else
                MessageDialog.ok(null, "Trenutno nema rezervacija za check in/out!", "Obaveštenje", JOptionPane.INFORMATION_MESSAGE);
        });
        createGuestItem.addActionListener(e -> {
            new AddEditGuestDialog(this, app.userManager, null);
        });

        User user = app.getUser();
        JLabel welcomeLabel = new JLabel("Dobrodošli " + user.getName() + " " + user.getSurname() + ".");
        welcomeLabel.setHorizontalAlignment(JLabel.CENTER);
        welcomeLabel.setVerticalAlignment(JLabel.CENTER);
        add(welcomeLabel);
    }
}
