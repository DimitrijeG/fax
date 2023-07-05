package table.model;

import javax.swing.table.AbstractTableModel;
import java.util.ArrayList;

public abstract class TableModel extends AbstractTableModel {
    private final String[] columnNames;

    public TableModel(String[] columnNames) {
        this.columnNames = setWrappingColumnNames(columnNames);
    }

    protected Object getAtIndex(int index) {
        return getData().get(index);
    }

    protected abstract ArrayList<Object> getData();

    @Override
    public int getRowCount() {
        return getData().size();
    }

    @Override
    public int getColumnCount() {
        return columnNames.length;
    }

    @Override
    public String getColumnName(int column) {
        return this.columnNames[column];
    }

    @Override
    public Class<?> getColumnClass(int columnIndex) {
        return this.getValueAt(0, columnIndex).getClass();
    }

    private String[] setWrappingColumnNames(String[] columnNames) {
        String[] wrappingColumns = columnNames.clone();
        for (int i = 0; i < columnNames.length; i++) {
            wrappingColumns[i] = "<html>" + columnNames[i] + "</html>";
        }
        return wrappingColumns;
    }
}
