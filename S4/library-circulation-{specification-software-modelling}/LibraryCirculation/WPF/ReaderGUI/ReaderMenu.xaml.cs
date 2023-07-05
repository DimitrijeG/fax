using System.ComponentModel;
using System.Windows;
using LibraryCirculation.WPF.Common;
using LibraryCirculation.WPF.DefaultGUI;
using LibraryCirculation.WPF.ReaderGUI.Reservations;

namespace LibraryCirculation.WPF.ReaderGUI
{
    public partial class ReaderMenu
    {
        private readonly Window _parentWindow;

        public ReaderMenu(Window parentWindow)
        {
            InitializeComponent();

            _parentWindow = parentWindow;
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _parentWindow.Show();
        }

        private void BtnBookRating_Click(object sender, RoutedEventArgs e)
        {
            new BookReportView().Show();
        }

        private void BtnReservation_Click(object sender, RoutedEventArgs e)
        {
            new CreateReservationView().ShowDialog();
        }

        private void BtnNotImplemented_Click(object sender, RoutedEventArgs e)
        {
            ViewUtil.ShowInformation("Funkcionalnost nije implementirana.");
        }
    }
}