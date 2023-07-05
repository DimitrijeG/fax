package table.renderer;

import entity.price.PriceSet;
import main.Settings;
import util.ViewUtil;

import javax.swing.*;
import javax.swing.table.TableCellRenderer;
import java.awt.*;

public class PriceSetTableCellRenderer extends JList<String> implements TableCellRenderer {

    @Override
    public Component getTableCellRendererComponent(JTable table, Object value, boolean isSelected, boolean hasFocus, int row, int column) {
        if (value == null)
            setListData(new String[]{});
        else if (value instanceof PriceSet)
            setListData(ViewUtil.toPriceArray(((PriceSet) value).getPrices()));
        if (isSelected)
            setBackground(UIManager.getColor("Table.selectionBackground"));
        else
            setBackground(UIManager.getColor("Table.background"));
        ViewUtil.changeFont(this, Settings.font);
        return this;
    }
}
