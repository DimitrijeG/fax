package view.dialog;

import javax.swing.*;
import java.awt.*;

public class MessageDialog {
    public static void ok(Component parent, String message, String title, int option) {
        String[] responses = {"Ok"};
        JOptionPane.showOptionDialog(
                parent,
                message,
                title,
                JOptionPane.DEFAULT_OPTION,
                option,
                null,
                responses,
                responses[0]);
    }

    public static boolean yesNo(Component parent, String message, String title, int option) {
        String[] responses = {"Da", "Ne"};
        int answer = JOptionPane.showOptionDialog(
                parent,
                message,
                title,
                JOptionPane.OK_CANCEL_OPTION,
                option,
                null,
                responses,
                responses[0]);
        return answer == JOptionPane.OK_OPTION;
    }

    public static boolean confirm(Component parent, String message) {
        return yesNo(parent, message, "Potvrda akcije", JOptionPane.PLAIN_MESSAGE);
    }

    public static boolean suggestId(Component parent, Integer suggestedId, String eMessage) {
        return yesNo(parent,
                eMessage +
                        " Da li prihvatate\npreporučeni id: " + suggestedId.toString() + "?",
                "Upozorenje", JOptionPane.WARNING_MESSAGE
        );
    }

    public static void tableRowNotSelected() {
        ok(null, "Niste izabrali red.", "Upozorenje", JOptionPane.WARNING_MESSAGE);
    }
}
