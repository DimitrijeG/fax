using System.ComponentModel;
using System.Windows;
using LibraryCirculation.WPF.Common;
using LibraryCirculation.WPF.DefaultGUI;

namespace LibraryCirculation.WPF.ChiefLibrarianGUI
{
    public partial class ChiefLibrarianMenu
    {
        private readonly Window _parentWindow;

        public ChiefLibrarianMenu(Window parentWindow)
        {
            InitializeComponent();
            _parentWindow = parentWindow;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _parentWindow.Show();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnBookRating_Click(object sender, RoutedEventArgs e)
        {
            new BookReportView().Show();
        }

        private void BtnNotImplemented_Click(object sender, RoutedEventArgs e)
        {
            ViewUtil.ShowInformation("Funkcionalnost nije implementirana.");
        }
    }
}