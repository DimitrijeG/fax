package view.generic;

import manager.CoreManager;
import view.frame.LoginFrame;

import javax.swing.*;
import java.io.IOException;

public class ExitMenu extends JMenu {
    public ExitMenu(JFrame parent, CoreManager app) {
        super();
        setText("Izađi");
        JMenuItem logout = new JMenuItem("Odjavi se");
        JMenuItem exit = new JMenuItem("Ugasi aplikaciju");

        add(logout);
        add(exit);

        logout.addActionListener(e -> {
            parent.dispose();
            app.setUser();
            new LoginFrame(app);
        });
        exit.addActionListener(e -> {
            try {
                app.saveData();
            } catch (IOException ex) {
                System.out.println("Unable to save data.");
                System.exit(0);
            }
            parent.dispose();
            System.exit(0);
        });
    }
}
