package view.frame;

import entity.user.User;
import main.Settings;
import manager.CoreManager;
import view.frame.table.MaidRoomTableFrame;
import view.generic.ExitMenu;
import view.generic.Window;

import javax.swing.*;
import java.awt.*;

import static util.ViewUtil.changeFont;

public class MaidFrame extends JFrame implements Window {
    CoreManager app;

    public MaidFrame(CoreManager app) {
        super();
        this.app = app;
        initialize();
    }

    @Override
    public void initialize() {
        setTitle("Sobarica");
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
        JMenuItem roomsItem = new JMenuItem("Sobe za spremanje");

        menuBar.add(roomsItem);
        menuBar.add(new ExitMenu(this, app));
        changeFont(menuBar, Settings.font);
        add(menuBar, BorderLayout.NORTH);


        // Klikom na stavke menija otvaraju se odgovarajuce forme za prikaz
        roomsItem.addActionListener(e -> {
            new MaidRoomTableFrame(app);
        });

        User user = app.getUser();
        JLabel welcomeLabel = new JLabel("Dobrodošli " + user.getName() + " " + user.getSurname() + ".");
        welcomeLabel.setHorizontalAlignment(JLabel.CENTER);
        welcomeLabel.setVerticalAlignment(JLabel.CENTER);
        add(welcomeLabel);
    }
}
