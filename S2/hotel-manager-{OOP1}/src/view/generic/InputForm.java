package view.generic;

import type.ValidatorException;

import javax.swing.*;
import java.awt.*;

public interface InputForm extends Window {
    void validateFields() throws ValidatorException;

    void addObject();

    void updateObject();

    void populate();

    Object collectData();

    boolean confirmEdit();

    void close();

    JLabel failStatus = new JLabel(" ");

    JButton cancelButton = new JButton("Odustani");

    default JButton initAdd(String text) {
        initCommon();
        return initSubmitActionAdd(text);
    }

    default JButton initEdit(String text) {
        populate();
        initCommon();
        return initSubmitActionEdit(text);
    }

    default void initCommon() {
        failStatus.setForeground(Color.red);
        failStatus.setOpaque(true);
        failStatus.setText(" ");
        cancelButton.addActionListener(e -> {
            close();
        });
    }

    default JButton initSubmitActionAdd(String text) {
        JButton submitButton = new JButton(text);
        submitButton.addActionListener(e -> {
            failStatus.setText(" ");
            try {
                validateFields();
                addObject();
                close();
            } catch (ValidatorException ex) {
                failStatus.setText(ex.getMessage());
            }
        });
        return submitButton;
    }

    default JButton initSubmitActionEdit(String text) {
        JButton submitButton = new JButton(text);
        submitButton.addActionListener(e -> {
            failStatus.setText(" ");
            try {
                validateFields();
                if (confirmEdit()) {
                    updateObject();
                    close();
                }
            } catch (ValidatorException ex) {
                failStatus.setText(ex.getMessage());
            }
        });
        return submitButton;
    }
}
