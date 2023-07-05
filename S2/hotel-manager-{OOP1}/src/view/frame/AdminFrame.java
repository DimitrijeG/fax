package view.frame;

import entity.user.User;
import main.Settings;
import manager.CoreManager;
import util.Report;
import view.frame.table.crud.*;
import view.generic.ExitMenu;
import view.generic.Window;

import javax.swing.*;
import java.awt.*;

import static util.ViewUtil.changeFont;

public class AdminFrame extends JFrame implements Window {
    CoreManager app;

    public AdminFrame(CoreManager app) {
        super();
        this.app = app;
        initialize();
    }

    @Override
    public void initialize() {
        setTitle("Admin");
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
        JMenu usersMenu = new JMenu("Korisnik");
        JMenuItem guestsItem = new JMenuItem("Gosti");
        JMenuItem employeesItem = new JMenuItem("Zaposleni");
        usersMenu.add(guestsItem);
        usersMenu.add(employeesItem);
        changeFont(usersMenu, Settings.font);
        JMenu articlesMenu = new JMenu("Artikli");
        JMenuItem roomTypesItem = new JMenuItem("Tipovi soba");
        JMenuItem addServicesItem = new JMenuItem("Dodatne usluge");
        JMenuItem amenitiesItem = new JMenuItem("Dodatna oprema");
        articlesMenu.add(roomTypesItem);
        articlesMenu.add(addServicesItem);
        articlesMenu.add(amenitiesItem);
        changeFont(articlesMenu, Settings.font);
        JMenuItem roomsItem = new JMenuItem("Sobe");
        JMenuItem reservationItem = new JMenuItem("Rezervacije");
        JMenuItem reportsItem = new JMenuItem("Izveštaji");

        menuBar.add(usersMenu);
        menuBar.add(articlesMenu);
        menuBar.add(roomsItem);
        menuBar.add(reservationItem);
        menuBar.add(reportsItem);
        menuBar.add(new ExitMenu(this, app));
        changeFont(menuBar, Settings.font);
        add(menuBar, BorderLayout.NORTH);

        guestsItem.addActionListener(e -> {
            new GuestTableFrame(app.userManager);
        });
        employeesItem.addActionListener(e -> {
            new EmployeeTableFrame(app.userManager);
        });
        roomsItem.addActionListener(e -> {
            new RoomTableFrame(app);
        });
        reservationItem.addActionListener(e -> {
            new ReservationTableFrame(app);
        });
        roomTypesItem.addActionListener(e -> {
            new ArticleTableFrame(app.roomTypeManager);
        });
        addServicesItem.addActionListener(e -> {
            new ArticleTableFrame(app.addServiceManager);
        });
        amenitiesItem.addActionListener(e -> {
            new ArticleTableFrame(app.amenityManager);
        });
        reportsItem.addActionListener(e -> {
            new Reports(new Report(app));
        });

        User user = app.getUser();
        JLabel welcomeLabel = new JLabel("Dobrodošli " + user.getName() + " " + user.getSurname() + ".");
        welcomeLabel.setHorizontalAlignment(JLabel.CENTER);
        welcomeLabel.setVerticalAlignment(JLabel.CENTER);
        add(welcomeLabel);
    }
}
