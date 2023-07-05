package main;

import manager.CoreManager;
import view.frame.LoginFrame;

import java.io.IOException;

public class HotelManager {

    public static void main(String[] args) {
        CoreManager app;
        try {
            app = new CoreManager("data");
        } catch (IOException e) {
            System.out.println(e.getMessage());
            return;
        }
//        new Reports(new ReportManager(app));
        new LoginFrame(app);
//        new GuestFrame(app);
//        new AdminFrame(app);
//        new ReceptionistFrame(app);
//        new MaidFrame(app);
    }
}
