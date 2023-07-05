package view.dialog.crud;

import entity.user.Guest;
import entity.user.User;
import main.Settings;
import manager.UserManager;
import net.miginfocom.swing.MigLayout;
import type.ValidatorException;
import type.enumeration.Gender;
import util.Validate;
import util.ViewUtil;
import view.dialog.MessageDialog;
import view.generic.InputForm;

import javax.swing.*;
import javax.swing.event.DocumentEvent;
import javax.swing.event.DocumentListener;
import javax.swing.text.JTextComponent;
import java.awt.*;
import java.time.LocalDate;

import static util.ViewUtil.*;

public class AddEditGuestDialog extends JDialog implements InputForm {
    protected final UserManager userManager;
    protected User initData;
    protected Gender selectedGender = Gender.OTHER;
    protected JTextField usernameField = new JTextField(), nameField = new JTextField();
    protected JTextField surnameField = new JTextField(), phoneField = new JTextField();
    protected JTextField addressField = new JTextField(), birthdayField = new JTextField();
    protected JPasswordField passwordField = new JPasswordField();
    protected JRadioButton maleRadio = new JRadioButton("muško");
    protected JRadioButton femaleRadio = new JRadioButton("žensko");
    protected JRadioButton otherRadio = new JRadioButton("drugo");
    private JTextField emailField = new JTextField(), passportField = new JTextField();

    public AddEditGuestDialog(JFrame parent, UserManager userManager, User user) {
        super(parent, true);
        this.userManager = userManager;
        this.initData = user;

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

        JButton submitButton;
        if (initData == null) {
            submitButton = initAdd("Dodaj");
        } else {
            submitButton = initEdit("Izmeni");
        }
        failStatus.setForeground(Color.red);

        JPanel genderPanel = ViewUtil.groupButtons(
                new AbstractButton[]{maleRadio, femaleRadio, otherRadio});
        maleRadio.addActionListener(e -> selectedGender = Gender.MALE);
        femaleRadio.addActionListener(e -> selectedGender = Gender.FEMALE);
        otherRadio.addActionListener(e -> selectedGender = Gender.OTHER);

        add(new Label("Ime:"));
        add(nameField, "growx");
        add(new Label("Prezime:"));
        add(surnameField, "growx");
        add(new Label("Korisnicko ime:"));
        add(usernameField, "growx");
        add(new Label("Lozinka:"));
        add(passwordField, "growx");
        add(new Label("Broj telefona:"));
        add(phoneField, "growx");
        add(new Label("Adresa:"));
        add(addressField, "growx");
        add(new Label("Datum rođenja:"));
        add(birthdayField, "growx");
        add(new Label("Pol:"));
        add(genderPanel, "growx");
        add(new Label("Email:"));
        add(emailField, "growx");
        add(new Label("Broj pasoša:"));
        add(passportField, "growx");

        if (initData == null) {
            setTitle("Dodavanje gosta");
            usernameField.setEditable(false);
            passwordField.setEditable(false);
            pairFields(emailField, usernameField);
            pairFields(passportField, passwordField);
        } else {
            setTitle("Izmena gosta");
        }

        add(failStatus, "growx,span 2,align center");
        add(submitButton);
        add(cancelButton);

        changeFont(this, Settings.font);
        pack();
        setVisible(true);
    }

    @Override
    public void validateFields() throws ValidatorException {
        for (JTextField textField : new JTextField[]{nameField, surnameField, phoneField, addressField}) {
            Validate.notEmpty(getText(textField));
        }
        Validate.username(getText(usernameField));
        String username = getText(usernameField);
        if (initData != null && !username.equals(initData.getUsername()))
            Validate.username(username, userManager.keys());
        Validate.password(getText(passwordField));
        String dateStr = getText(birthdayField);
        Validate.localDate(dateStr);
        if (!ViewUtil.parseDate(dateStr).isBefore(LocalDate.now()))
            throw new ValidatorException("Datum rođenja nije validan.");
        Validate.email(getText(emailField));
        Validate.passport(getText(passportField));
    }

    protected void populateBasic() {
        nameField.setText(initData.getName());
        surnameField.setText(initData.getSurname());
        phoneField.setText(initData.getPhone());
        addressField.setText(initData.getAddress());
        usernameField.setText(initData.getUsername());
        passwordField.setText(initData.getPassword());
        birthdayField.setText(ViewUtil.toString(initData.getBirthday()));
        switch (initData.getGender()) {
            case MALE:
                maleRadio.setSelected(true);
                break;
            case FEMALE:
                femaleRadio.setSelected(true);
                break;
            case OTHER:
                otherRadio.setSelected(true);
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


    @Override
    public void addObject() {
        Guest g = (Guest) collectData();
        userManager.addUser(g);
    }

    @Override
    public void updateObject() {
        Guest g = (Guest) collectData();
        userManager.updateUser(initData.getUsername(), g);
    }

    @Override
    public Object collectData() {
        String name = getText(nameField);
        String surname = getText(surnameField);
        String phone = getText(phoneField);
        String address = getText(addressField);
        String username = getText(usernameField);
        String password = getText(passwordField);
        LocalDate birthday = parseDate(getText(birthdayField));
        Gender gender = selectedGender;
        String email = getText(emailField);
        String passport = getText(passportField);
        return new Guest(name, surname, phone, address, username, password, birthday, gender, email, passport);
    }

    @Override
    public void populate() {
        populateBasic();
        Guest g = (Guest) initData;
        emailField.setText(g.getEmail());
        passportField.setText(g.getPassport());
    }

    private void pairFields(JTextComponent original, JTextComponent copy) {
        original.getDocument().addDocumentListener(new DocumentListener() {
            @Override
            public void insertUpdate(DocumentEvent e) {
                updatePair();
            }

            @Override
            public void removeUpdate(DocumentEvent e) {
                updatePair();
            }

            @Override
            public void changedUpdate(DocumentEvent e) {
                updatePair();
            }

            private void updatePair() {
                copy.setText(original.getText());
            }
        });
    }
}
