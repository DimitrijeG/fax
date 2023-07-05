using System;
using System.ComponentModel;
using System.Windows;
using HealthCare.WPF.ManagerGUI.HumanResources;
using HealthCare.WPF.ManagerGUI.Interior.Renovations;
using HealthCare.WPF.ManagerGUI.PatientSatisfaction.Analytics;
using HealthCare.WPF.ManagerGUI.PhysicalAssets.Inventory;
using HealthCare.WPF.ManagerGUI.PhysicalAssets.Orders;
using HealthCare.WPF.ManagerGUI.PhysicalAssets.Rearranging;

namespace HealthCare.WPF.ManagerGUI
{
    public partial class ManagerMenu : Window
    {
        private MainWindow _loginWindow;

        public ManagerMenu(MainWindow loginWindow)
        {
            InitializeComponent();
            _loginWindow = loginWindow;
        }

        private void Button_Equipment(object sender, EventArgs e)
        {
            new InventoryListingView().ShowDialog();
        }

        private void Button_Ordering(object sender, RoutedEventArgs e)
        {
            new EquipmentOrderView().ShowDialog();
        }

        private void Button_Rearranging(object sender, RoutedEventArgs e)
        {
            new EquipmentRearrangingView().ShowDialog();
        }

        private void Button_Renovation(object sender, RoutedEventArgs e)
        {
            new RenovationListingView().ShowDialog();
        }

        private void Button_AbsenceRequests(object sender, RoutedEventArgs e)
        {
            new AbsenceRequestListingView().ShowDialog();
        }

        private void Button_Analytics(object sender, RoutedEventArgs e)
        {
            new AnalyticsMainView().ShowDialog();
        }

        private void Button_Logout(object sender, RoutedEventArgs e)
        {
            Close();
            _loginWindow.Show();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _loginWindow.Show();
        }
    }
}