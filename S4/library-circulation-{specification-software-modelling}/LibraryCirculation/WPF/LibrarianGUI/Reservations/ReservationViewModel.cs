using LibraryCirculation.Application;
using LibraryCirculation.Application.Common;
using LibraryCirculation.Core.Fund.Books;
using LibraryCirculation.Core.Renting;
using LibraryCirculation.Core.Users.Readers;
using LibraryCirculation.WPF.Common;

namespace LibraryCirculation.WPF.LibrarianGUI.Reservations
{
    public class ReservationViewModel
    {
        private readonly Reservation _reservation;

        public ReservationViewModel(Reservation reservation)
        {
            _reservation = reservation;
            var reader = Injector.GetService<ReaderService>().Get(reservation.ReaderUsername);
            ReaderName = reader.Name + " " + reader.Surname;
        }

        public int Id => _reservation.Id;
        public string SubmittingDate => Util.ToString(_reservation.SubmittingDate, Formats.Date);
        public string ApprovalDate => Util.ToString(_reservation.CopyAssignmentDate, Formats.Date);
        public string Status => ViewUtil.Translate(_reservation.Status);
        public string BookName => Injector.GetService<BookService>().Get(_reservation.BookIsbn).Title;
        public string ReaderName { get; }
        public int AssignedCopyId => _reservation.AssignedCopyId;
    }
}