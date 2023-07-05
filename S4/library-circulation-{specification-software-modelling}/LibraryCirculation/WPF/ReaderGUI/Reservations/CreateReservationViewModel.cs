using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LibraryCirculation.Application;
using LibraryCirculation.Core.Fund.Books;
using LibraryCirculation.Core.Renting;
using LibraryCirculation.WPF.Common;

namespace LibraryCirculation.WPF.ReaderGUI.Reservations
{
    public class CreateReservationViewModel : ViewModelBase
    {
        public List<Book> Books { get; }
        public Reservation Reservation { get; }
        public ExitCommand ExitCommand { get; }
        public RelayCommand CreateCommand { get; }

        public CreateReservationViewModel(Window current)
        {
            Books = Injector.GetService<BookService>().GetAll();
            Reservation = new Reservation
            {
                BookIsbn = Books.First().Isbn,
                ReaderUsername = Context.Current.Username
            };

            ExitCommand = new ExitCommand(current);
            CreateCommand = new RelayCommand(_ =>
            {
                Reservation.SubmittingDate = DateTime.Now;
                Injector.GetService<ReservationService>().Add(Reservation);
                ViewUtil.ShowInformation("Rezervacija uspešno kreirana i poslata bibliotekaru.");
                ExitCommand.Execute();
            });
        }
    }
}
