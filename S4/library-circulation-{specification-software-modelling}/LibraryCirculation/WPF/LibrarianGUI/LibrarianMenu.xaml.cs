using System.ComponentModel;
using System.Windows;
using LibraryCirculation.WPF.Common;
using LibraryCirculation.WPF.DefaultGUI;
using LibraryCirculation.WPF.LibrarianGUI.Books;
using LibraryCirculation.WPF.LibrarianGUI.Copies;
using LibraryCirculation.WPF.LibrarianGUI.Reservations;

namespace LibraryCirculation.WPF.LibrarianGUI
{
    public partial class LibrarianMenu
    {
        private readonly Window _parentWindow;

        public LibrarianMenu(Window parentWindow)
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

        private void BtnBooks_Click(object sender, RoutedEventArgs e)
        {
            new BookManagementView().ShowDialog();
        }

        private void BtnCopies_Click(object sender, RoutedEventArgs e)
        {
            new CopyManagementView().ShowDialog();
        }

        private void BtnBookRating_Click(object sender, RoutedEventArgs e)
        {
            new BookReportView().Show();
        }

        private void BtnReservations_Click(object sender, RoutedEventArgs e)
        {
            new ReservationListingView().ShowDialog();
        }

        private void BtnNotImplemented_Click(object sender, RoutedEventArgs e)
        {
            ViewUtil.ShowInformation("Funkcionalnost nije implementirana.");
        }
    }
}