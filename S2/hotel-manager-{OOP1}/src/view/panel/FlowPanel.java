package view.panel;

import javax.swing.*;
import java.awt.*;

public class FlowPanel {
    public static JPanel getPanel() {
        return new JPanel(new FlowLayout() {
            public Dimension preferredLayoutSize(Container target) {
                Dimension sd = super.preferredLayoutSize(target);
                sd.width = Math.min(200, sd.width);
                return sd;
            }
        });
    }
}
