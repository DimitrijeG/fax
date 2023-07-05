package table.renderer;

import main.Settings;
import type.DateRange;
import util.ViewUtil;

import javax.swing.*;
import javax.swing.table.DefaultTableCellRenderer;
import java.awt.*;
import java.time.LocalDate;

public class DateTableCellRenderer extends DefaultTableCellRenderer {

    @Override
    public Component getTableCellRendererComponent(JTable table, Object value, boolean isSelected, boolean hasFocus, int row, int column) {
        if (value instanceof LocalDate)
            setText(ViewUtil.toString((LocalDate) value));
        else if (value instanceof DateRange)
            setText(ViewUtil.toString((DateRange) value));
        if (isSelected)
            setBackground(UIManager.getColor("Table.selectionBackground"));
        else
            setBackground(UIManager.getColor("Table.background"));
        ViewUtil.changeFont(this, Settings.font);
        return this;
    }
}
