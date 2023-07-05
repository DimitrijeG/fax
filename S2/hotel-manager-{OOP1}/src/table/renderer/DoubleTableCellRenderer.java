package table.renderer;

import main.Settings;
import util.ViewUtil;

import javax.swing.*;
import javax.swing.table.DefaultTableCellRenderer;
import java.awt.*;

import static util.ViewUtil.changeFont;

public class DoubleTableCellRenderer extends DefaultTableCellRenderer {

    public DoubleTableCellRenderer() {
        setHorizontalAlignment(SwingConstants.RIGHT);
    }

    @Override
    public Component getTableCellRendererComponent(JTable table, Object value, boolean isSelected, boolean hasFocus, int row, int column) {
        if (value instanceof Double)
            setText(ViewUtil.toString((Double) value));
        if (isSelected)
            setBackground(UIManager.getColor("Table.selectionBackground"));
        else
            setBackground(UIManager.getColor("Table.background"));
        changeFont(this, Settings.font);
        return this;
    }
}
