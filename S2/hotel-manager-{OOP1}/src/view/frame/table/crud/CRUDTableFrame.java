package view.frame.table.crud;

import net.miginfocom.swing.MigLayout;
import view.frame.table.TableFrame;

import javax.swing.*;
import javax.swing.table.TableModel;
import java.awt.*;

public abstract class CRUDTableFrame extends TableFrame {
    protected JButton buttonAdd;
    protected JButton buttonEdit;
    protected JButton buttonDelete;

    public CRUDTableFrame(TableModel model) {
        super(model);
    }

    @Override
    protected void initSpecificToolBar() {
        mainToolbar.setLayout(new MigLayout(
                "wrap 1", "[center]", "[b][b][b]"
        ));

        buttonAdd = new JButton("Dodaj");
        buttonEdit = new JButton("Izmeni");
        buttonDelete = new JButton("Obriši");

        ImageIcon addIcon = new ImageIcon("img/add.png");
        addIcon = new ImageIcon(addIcon.getImage().getScaledInstance(20, 20, Image.SCALE_SMOOTH));
        buttonAdd.setIcon(addIcon);
        ImageIcon editIcon = new ImageIcon("img/edit.gif");
        buttonEdit.setIcon(editIcon);
        ImageIcon deleteIcon = new ImageIcon("img/remove.gif");
        buttonDelete.setIcon(deleteIcon);

        mainToolbar.add(buttonAdd);
        mainToolbar.add(buttonEdit);
        mainToolbar.add(buttonDelete);
    }
}
