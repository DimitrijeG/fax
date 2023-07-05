package table.renderer;

import main.Settings;

import javax.swing.*;
import javax.swing.table.TableCellRenderer;
import java.awt.*;

import static util.ViewUtil.changeFont;

public class MultilineTableCellRenderer extends JList<String> implements TableCellRenderer {

    @Override
    public Component getTableCellRendererComponent(JTable table, Object value, boolean isSelected, boolean hasFocus, int row, int column) {
        if (value instanceof String[])
            setListData((String[]) value);
        if (isSelected)
            setBackground(UIManager.getColor("Table.selectionBackground"));
        else
            setBackground(UIManager.getColor("Table.background"));
        changeFont(this, Settings.font);
        return this;
    }
}
