package view.frame;

import main.Settings;
import util.Report;
import net.miginfocom.swing.MigLayout;
import org.apache.commons.lang.StringUtils;
import type.DateRange;
import type.EmptyFieldException;
import type.ValidatorException;
import util.Validate;
import util.ViewUtil;
import view.generic.Window;

import javax.swing.*;
import java.awt.*;
import java.time.LocalDate;
import java.util.ArrayList;

import static util.ViewUtil.changeFont;
import static util.ViewUtil.getText;

public class Reports extends JFrame implements Window {
    Report reports;
    JLabel failField = new JLabel(), incomeField = new JLabel(), expenditureField = new JLabel();
    JLabel pendingField = new JLabel(), approvedField = new JLabel(), declinedField = new JLabel(), withdrewField = new JLabel();
    JTextField startField = new JTextField(), endField = new JTextField();
    JTextArea roomsArea = new JTextArea(), maidsArea = new JTextArea();
    JButton showStatsButton = new JButton("Prikaži");


    public Reports(Report reports) {
        super();
        this.reports = reports;
        initialize();
    }

    @Override
    public void initialize() {
        setTitle("Izveštaji");
        setDefaultCloseOperation(JDialog.DISPOSE_ON_CLOSE);
        setBackground(Color.lightGray);
//        setPreferredSize(new Dimension(400, 700));

        initAction();
        initGui();

        pack();
        setLocation(300, 20);
        changeFont(this, Settings.font);
        setVisible(true);
    }

    private void initGui() {
        setLayout(new BorderLayout());
        JPanel panel = new JPanel(new MigLayout(
                "wrap 2,insets 10 15 10 10",
                "[right]8[left]",
                "[]15[]8[]8[]8[]30[]8[]30[]8[]8[]8[]8[]30[]10[]30[]10[]"
        ));
        failField.setForeground(Color.red);
        failField.setOpaque(true);
        failField.setText(" ");
        startField.setPreferredSize(new Dimension(200, 40));
        endField.setPreferredSize(new Dimension(200, 40));
        roomsArea.setRows(7);
        maidsArea.setRows(7);
        roomsArea.setEditable(false);
        maidsArea.setEditable(false);

        panel.add(new Label("Opseg datuma za izveštaje:"), "span 2,align center");
        panel.add(new Label("početak: "));
        panel.add(startField);
        panel.add(new Label("kraj: "));
        panel.add(endField);
        panel.add(failField, "span 2,align center");
        panel.add(showStatsButton, "span 2,align center");


        panel.add(new Label("Prihodi (RSD): "));
        panel.add(incomeField);
        panel.add(new Label("Rashodi (RSD): "));
        panel.add(expenditureField);
        panel.add(new Label("Rezervacije"), "span 2,align center");
        panel.add(new Label("Broj obrađenih: "));
        panel.add(pendingField);
        panel.add(new Label("Broj potvrđenih: "));
        panel.add(approvedField);
        panel.add(new Label("Broj odbijenih: "));
        panel.add(declinedField);
        panel.add(new Label("Broj otkazanih: "));
        panel.add(withdrewField);
        panel.add(new Label("Prikaz soba"), "span 2,align center");
        panel.add(roomsArea, "growx,growy,span 2,align center");
        panel.add(new Label("Rad sobarica"), "span 2,align center");
        panel.add(maidsArea, "growx,growy,span 2,align center");

        JScrollPane sp = new JScrollPane(panel);
        sp.setPreferredSize(new Dimension(600, 800));
        add(sp, BorderLayout.CENTER);
    }

    private void initAction() {
        showStatsButton.addActionListener(e -> {
            failField.setText(" ");
            try {
                validation();
                LocalDate start = LocalDate.MIN;
                LocalDate end = LocalDate.MAX;
                if (!getText(startField).isEmpty())
                    start = ViewUtil.parseDate(getText(startField));
                if (!getText(endField).isEmpty())
                    end = ViewUtil.parseDate(getText(endField));

                DateRange range = new DateRange(start, end);

                incomeField.setText(ViewUtil.toString(reports.getIncome(range)));
                expenditureField.setText(ViewUtil.toString(reports.getExpenditure(range)));

                pendingField.setText(reports.getProcessedReservations(range).toString());
                approvedField.setText(reports.getApprovedReservations(range).toString());
                declinedField.setText(reports.getDeclinedReservations(range).toString());
                withdrewField.setText(reports.getWithdrewReservations(range).toString());

                loadRoomStats(range);
                loadMaidStats(range);
                setPreferredSize(getPreferredSize());
            } catch (ValidatorException ex) {
                failField.setText(ex.getMessage());
            }
        });
    }

    private void validation() throws ValidatorException {
        try {
            Validate.localDate(getText(startField));
            Validate.localDate(getText(endField));
            LocalDate start = ViewUtil.parseDate(getText(startField));
            LocalDate end = ViewUtil.parseDate(getText(endField));
            if (!start.isBefore(end)) throw new ValidatorException("Početni datum mora da bude pre krajnjeg.");
        } catch (EmptyFieldException ignored) {
        }
    }

    private void loadRoomStats(DateRange range) {
        ArrayList<Object[]> roomStats = reports.getRoomStats(range);
        String[] columns = new String[] {
                "id sobe", "tip sobe", "noćenja", "dobit (RSD)"
        };
        roomsArea.setText(StringUtils.center(columns[0], columns[0].length()) + " | " +
                StringUtils.center(columns[1], columns[1].length()) + " | " +
                StringUtils.center(columns[2], columns[2].length()) + " | " +
                StringUtils.center(columns[3], columns[3].length()) + "\n");

        String id, type, nights, income;

        for (Object[] row : roomStats) {
            id = ViewUtil.formatId((Integer) row[0], Settings.roomIdLength);
            type = ViewUtil.formatId((Integer) row[1], Settings.articleIdLength);
            nights = ((Integer) row[2]).toString();
            income = ViewUtil.toString((Double) row[3]);
            roomsArea.append(StringUtils.center(id, columns[0].length()) + " | " +
                    StringUtils.center(type, columns[1].length()) + " | " +
                    StringUtils.center(nights, columns[2].length()) + " | " +
                    StringUtils.center(income, columns[3].length()) + "\n");
        }
    }

    private void loadMaidStats(DateRange range) {
        ArrayList<Object[]> maidStats = reports.getMaidStats(range);

        String[] columns = new String[] {
                "username", "ime", "prezime", "spremljenih soba"
        };
        int maxUsername = 0, maxName = 0, maxSurname = 0;
        for (Object[] row : maidStats) {
            maxUsername = Math.max(maxUsername, Math.max(((String) row[0]).length(), columns[0].length()));
            maxName = Math.max(maxName, Math.max(((String) row[1]).length(), columns[1].length()));
            maxSurname = Math.max(maxSurname, Math.max(((String) row[2]).length(), columns[2].length()));
        }

        maidsArea.setText(StringUtils.center(columns[0], maxUsername) + " | " +
                StringUtils.center(columns[1], maxName) + " | " +
                StringUtils.center(columns[2], maxSurname) + " | " +
                StringUtils.center(columns[3], columns[3].length()) + "\n");
        for (Object[] row : maidStats) {
            maidsArea.append(StringUtils.center((String) row[0], maxUsername) + " | " +
                    StringUtils.center((String) row[1], maxName) + " | " +
                    StringUtils.center((String) row[2], maxSurname) + " | " +
                    ((Integer) row[3]).toString() + "\n");
        }
    }
}
