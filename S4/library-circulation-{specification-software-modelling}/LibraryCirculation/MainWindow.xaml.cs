using System;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using LibraryCirculation.Application;
using LibraryCirculation.Application.Exceptions;
using LibraryCirculation.Core.Common;
using LibraryCirculation.Core.Fund;
using LibraryCirculation.Core.Fund.Books;
using LibraryCirculation.Core.Renting;
using LibraryCirculation.Core.Users;
using LibraryCirculation.WPF.AdministratorGUI;
using LibraryCirculation.WPF.ChiefLibrarianGUI;
using LibraryCirculation.WPF.Common;
using LibraryCirculation.WPF.DefaultGUI;
using LibraryCirculation.WPF.LibrarianGUI;
using LibraryCirculation.WPF.ReaderGUI;

namespace LibraryCirculation
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (Injector.GetService<LoginService>().Login(TbUsername.Text, PbPassword.Password))
                {
                    case Role.Reader:
                        new ReaderMenu(this).Show();
                        break;
                    case Role.Librarian:
                        new LibrarianMenu(this).Show();
                        break;
                    case Role.ChiefLibrarian:
                        new ChiefLibrarianMenu(this).Show();
                        break;
                    case Role.Administrator:
                    default:
                        new AdministratorMenu(this).Show();
                        break;
                }

                TbUsername.Clear();
                PbPassword.Clear();
                Hide();
            }
            catch (LoginException ex)
            {
                ViewUtil.ShowWarning(ex.Message);
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            ExitApp(sender, e);
        }

        public void ExitApp(object sender, EventArgs e)
        {
            // GenerateRandomRentals();
            System.Windows.Application.Current.Shutdown();
        }

        private void GenerateRandomRentals()
        {
            var gen = new Random();
            var start = DateTime.Today.AddDays(-30);
            var range = (DateTime.Today - start).Days - 6;

            Injector.GetService<CopyService>().GetAll().GroupBy(c => c.BookIsbn)
                .Select(g => g.First().Id).ToList().ForEach(id =>
                {
                    var i = gen.Next(5, 40);

                    for (var j = 0; j < i; j++)
                    {
                        var readerId = gen.Next(1, 4);
                        var d1 = start.AddDays(gen.Next(1, range));
                        var d2 = d1.AddDays(gen.Next(1, 6));

                        Injector.GetService<RentalService>().Add(new Rental(0, new TimeSlot(d1, d2), true, id, readerId));
                    }
                });
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