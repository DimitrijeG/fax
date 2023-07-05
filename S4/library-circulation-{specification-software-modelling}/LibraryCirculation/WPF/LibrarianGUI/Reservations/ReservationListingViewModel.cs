using System.Collections.ObjectModel;
using LibraryCirculation.Application;
using LibraryCirculation.Core.Renting;

namespace LibraryCirculation.WPF.LibrarianGUI.Reservations
{
    public class ReservationListingViewModel
    {
        public ReservationListingViewModel()
        {
            Items = new ObservableCollection<ReservationViewModel>();
            LoadAll();
        }

        public ObservableCollection<ReservationViewModel> Items { get; set; }
        public ReservationViewModel? SelectedReservation { get; set; }

        public void LoadAll()
        {
            Items.Clear();
            foreach (var reservation in Injector.GetService<ReservationService>().GetAll())
                Items.Add(new ReservationViewModel(reservation));
            SelectedReservation = null;
        }
    }
}