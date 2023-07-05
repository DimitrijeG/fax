package view.frame;


import entity.user.Employee;
import entity.user.Guest;
import entity.user.User;
import main.Settings;
import manager.CoreManager;
import net.miginfocom.swing.MigLayout;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import static util.ViewUtil.changeFont;
import static util.ViewUtil.getText;

public class LoginFrame extends JFrame implements ActionListener {

    CoreManager app;
    JLabel failStatus = new JLabel(" ");
    JTextField usernameField = new JTextField();
    JPasswordField passwordField = new JPasswordField();
    JButton submitButton, cancelButton;

    public LoginFrame(CoreManager app) {
        super();
        this.app = app;
        initialize();
    }

    public void initialize() {
        setTitle("Prijava");
        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        LayoutManager layout = new MigLayout(
                "wrap 2,insets 15 30 15 30",
                "[170::,right]10[230::,left]",
                "[40::]20[][]10[]10[]");

        setLayout(layout);

        failStatus.setForeground(Color.red);

        submitButton = new JButton("Prijava");
        cancelButton = new JButton("Odustani");
        submitButton.addActionListener(this);
        cancelButton.addActionListener(this);

        add(new Label("Dobrodošli, unesite kredencijale"), "span 2,align center");

        add(new Label("Korisnicko ime:"));
        add(usernameField, "growx");
        add(new Label("Lozinka:"));
        add(passwordField, "growx");
        add(failStatus, "growx,span 2,align center");

        add(submitButton);
        add(cancelButton);

        changeFont(this, Settings.font);
        pack();
        setLocationRelativeTo(null);
        setVisible(true);
    }

    @Override
    public void actionPerformed(ActionEvent e) {
        if (e.getSource() == submitButton) {
            String username = getText(usernameField);
            String password = getText(passwordField);
            failStatus.setText(" ");

            if (username.isEmpty())
                failStatus.setText("Polje korisničko ime ne može da bude prazno!");
            else if (validCredentials(username, password)) {
                app.setUser(username);

                if (app.getUser() instanceof Guest)
                    new GuestFrame(app);
                else
                    switch (((Employee) app.getUser()).getRole()) {
                        case ADMIN:
                            new AdminFrame(app);
                            dispose();
                            break;
                        case RECEPTIONIST:
                            new ReceptionistFrame(app);
                            dispose();
                            break;
                        case MAID:
                            new MaidFrame(app);
                            dispose();
                    }
                dispose();
            } else
                failStatus.setText("Neispravno korisničko ime ili lozinka.");
        } else if (e.getSource() == cancelButton) {
            dispose();
            System.exit(0);
        }
    }

    private boolean validCredentials(String username, String password) {
        User user = app.userManager.getUser(username);
        return user != null && user.getPassword().equals(password);
    }
}
