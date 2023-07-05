package view.frame.table;

import entity.price.PriceSet;
import main.Settings;
import table.TableColumnAdjuster;
import table.renderer.DateTableCellRenderer;
import table.renderer.DoubleTableCellRenderer;
import table.renderer.MultilineTableCellRenderer;
import table.renderer.PriceSetTableCellRenderer;
import type.DateRange;

import javax.swing.*;
import javax.swing.table.AbstractTableModel;
import javax.swing.table.TableModel;
import javax.swing.table.TableRowSorter;
import java.awt.*;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.time.LocalDate;
import java.util.HashMap;

import static util.ViewUtil.changeFont;

public abstract class TableFrame extends JFrame {
    protected JTable table;
    protected JToolBar mainToolbar = new JToolBar();
    protected TableRowSorter<AbstractTableModel> tableSorter;
    protected HashMap<Integer, Integer> sortOrder = new HashMap<>();
    private TableColumnAdjuster columnAdjuster;

    public TableFrame(TableModel model) {
        setLocation(100, 100);
        setDefaultCloseOperation(JDialog.DISPOSE_ON_CLOSE);
        setBackground(Color.lightGray);

        initTable(model);
        initToolBar();
        initSortOrder(model.getColumnCount());
        initActions();

        pack();
        changeFont(this, Settings.font);
        setVisible(true);
    }

    private void initTable(TableModel model) {
        table = new JTable(model);
        table.setRowSorter(tableSorter);
        table.getSelectionModel().setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
        table.getTableHeader().setReorderingAllowed(false);
//        table.setAutoResizeMode(JTable.AUTO_RESIZE_OFF);
        table.setDefaultRenderer(String[].class, new MultilineTableCellRenderer());
        table.setDefaultRenderer(PriceSet.class, new PriceSetTableCellRenderer());
        table.setDefaultRenderer(Double.class, new DoubleTableCellRenderer());
        table.setDefaultRenderer(LocalDate.class, new DateTableCellRenderer());
        table.setDefaultRenderer(DateRange.class, new DateTableCellRenderer());
        table.getTableHeader().addMouseListener(new MouseAdapter() {
            @Override
            public void mouseClicked(MouseEvent arg0) {
                int index = table.getTableHeader().columnAtPoint(arg0.getPoint());
                sort(index);
            }
        });

        columnAdjuster = new TableColumnAdjuster(table);
        columnAdjuster.setDynamicAdjustment(true);
        columnAdjuster.setColumnDataIncluded(true);
        columnAdjuster.setColumnHeaderIncluded(true);

        tableSorter = new TableRowSorter<>();
        tableSorter.setModel((AbstractTableModel) table.getModel());

        updateTable();
        JScrollPane sc = new JScrollPane(table);
        add(sc, BorderLayout.CENTER);
    }

    private void initToolBar() {
        mainToolbar.setFloatable(false);
        mainToolbar.setLayout(new BorderLayout());
        initSpecificToolBar();
        add(mainToolbar, BorderLayout.EAST);
    }

    protected void initSortOrder(int columns) {
        for (int i = 0; i < columns; i++)
            sortOrder.put(i, 1);
    }

    public void updateTable() {
        AbstractTableModel sm = (AbstractTableModel) this.table.getModel();
        sm.fireTableDataChanged();
        columnAdjuster.adjustColumns();
        updateRowHeights(table);
    }

    private void updateRowHeights(JTable table) {
        for (int row = 0; row < table.getRowCount(); row++) {
            int rowHeight = table.getRowHeight();
            for (int column = 0; column < table.getColumnCount(); column++) {
                Component comp = table.prepareRenderer(table.getCellRenderer(row, column), row, column);
                rowHeight = Math.max(rowHeight, comp.getPreferredSize().height);
            }
            table.setRowHeight(row, rowHeight);
        }
    }

    private void sort(int index) {
        sortData(index);

        System.out.println("kolona " + index + " smer " + sortOrder.get(index));
        sortOrder.put(index, sortOrder.get(index) * -1);
        updateTable();
    }

    protected abstract void initSpecificToolBar();

    protected abstract void initActions();

    protected abstract void sortData(int index);
}
