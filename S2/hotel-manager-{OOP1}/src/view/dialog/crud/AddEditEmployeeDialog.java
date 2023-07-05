package view.dialog.crud;

import entity.user.Employee;
import entity.user.User;
import main.Settings;
import manager.UserManager;
import net.miginfocom.swing.MigLayout;
import type.ValidatorException;
import type.enumeration.EducationLvl;
import type.enumeration.EmployeeRole;
import type.enumeration.Gender;
import util.Validate;
import util.ViewUtil;
import view.dialog.MessageDialog;
import view.generic.InputForm;

import javax.swing.*;
import java.awt.*;
import java.time.LocalDate;

import static util.ViewUtil.*;

public class AddEditEmployeeDialog extends JDialog implements InputForm {
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

    private JTextField serviceYearsField = new JTextField(), salaryField = new JTextField();

    private EmployeeRole selectedRole = EmployeeRole.ADMIN;
    private EducationLvl selectedEducationLvl = EducationLvl.ZERO;
    JRadioButton adminRadio = new JRadioButton("administrator"), receptionistRadio = new JRadioButton("recepcioner"), maidRadio = new JRadioButton("sobarica");
    JRadioButton zeroEdRadio = new JRadioButton("nema"), firstEdRadio = new JRadioButton("prvi"), secondEdRadio = new JRadioButton("drugi"), thirdEdRadio = new JRadioButton("treći");

    public AddEditEmployeeDialog(JFrame parent, UserManager userManager, Employee employee) {
        super(parent, true);
        this.userManager = userManager;
        this.initData = employee;

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

        JPanel genderPanel = ViewUtil.groupButtons(
                new AbstractButton[]{maleRadio, femaleRadio, otherRadio});
        maleRadio.addActionListener(e -> selectedGender = Gender.MALE);
        femaleRadio.addActionListener(e -> selectedGender = Gender.FEMALE);
        otherRadio.addActionListener(e -> selectedGender = Gender.OTHER);

        JPanel rolePanel = ViewUtil.groupButtons(
                new AbstractButton[]{adminRadio, receptionistRadio, maidRadio});
        adminRadio.addActionListener(e -> selectedRole = EmployeeRole.ADMIN);
        receptionistRadio.addActionListener(e -> selectedRole = EmployeeRole.RECEPTIONIST);
        maidRadio.addActionListener(e -> selectedRole = EmployeeRole.MAID);

        JPanel educationPanel = ViewUtil.groupButtons(
                new AbstractButton[]{zeroEdRadio, firstEdRadio, secondEdRadio, thirdEdRadio});
        zeroEdRadio.addActionListener(e -> selectedEducationLvl = EducationLvl.ZERO);
        firstEdRadio.addActionListener(e -> selectedEducationLvl = EducationLvl.FIRST);
        secondEdRadio.addActionListener(e -> selectedEducationLvl = EducationLvl.SECOND);
        thirdEdRadio.addActionListener(e -> selectedEducationLvl = EducationLvl.THIRD);

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
        add(new Label("Uloga zaposlenog:"));
        add(rolePanel, "growx");
        add(new Label("Nivo stručne spreme:"));
        add(educationPanel, "growx");
        add(new Label("Staž:"));
        add(serviceYearsField, "growx");
        add(new Label("Plata (RSD):"));
        add(salaryField, "growx");

        if (initData == null)
            setTitle("Dodavanje radnika");
        else
            setTitle("Izmena radnika");

        add(failStatus, "growx,span 2,align center");
        JButton submitButton;
        if (initData == null) {
            submitButton = initAdd("Dodaj");
        } else {
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
        for (JTextField textField : new JTextField[]{nameField, surnameField, phoneField, addressField}) {
            Validate.notEmpty(getText(textField));
        }
        Validate.username(getText(usernameField));
        String username = getText(usernameField);
        if (!username.equals(initData.getUsername()))
            Validate.username(username, userManager.keys());
        Validate.password(getText(passwordField));
        String dateStr = getText(birthdayField);
        Validate.localDate(dateStr);
        if (!ViewUtil.parseDate(dateStr).isBefore(LocalDate.now()))
            throw new ValidatorException("Datum rođenja nije validan.");
        Validate.integer(getText(serviceYearsField));
        Validate.price(getText(salaryField));
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
        Employee employee = (Employee) collectData();
        userManager.addUser(employee);
    }

    @Override
    public void updateObject() {
        Employee employee = (Employee) collectData();
        userManager.updateUser(initData.getUsername(), employee);
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
        EmployeeRole role = selectedRole;
        EducationLvl educationLvl = selectedEducationLvl;
        Integer yearsOfService = ViewUtil.parseInteger(getText(serviceYearsField));
        Double salary = ViewUtil.parseDouble(getText(salaryField));
        return new Employee(name, surname, phone, address, username, password, birthday, gender, role, educationLvl, yearsOfService, salary);
    }

    @Override
    public void populate() {
        populateBasic();
        Employee e = (Employee) initData;
        switch (e.getRole()) {
            case ADMIN:
                adminRadio.setSelected(true);
                break;
            case RECEPTIONIST:
                receptionistRadio.setSelected(true);
                break;
            case MAID:
                maidRadio.setSelected(true);
        }
        switch (e.getEducationLvl()) {
            case ZERO:
                zeroEdRadio.setSelected(true);
                break;
            case FIRST:
                firstEdRadio.setSelected(true);
                break;
            case SECOND:
                secondEdRadio.setSelected(true);
                break;
            case THIRD:
                thirdEdRadio.setSelected(true);
        }
        serviceYearsField.setText(e.getYearsOfService().toString());
        salaryField.setText(ViewUtil.toString(e.getSalary()));
    }
}
