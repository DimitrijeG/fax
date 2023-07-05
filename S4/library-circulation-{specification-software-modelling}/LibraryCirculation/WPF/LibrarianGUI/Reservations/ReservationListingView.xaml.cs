using System;
using System.Runtime.CompilerServices;
using System.Windows;
using LibraryCirculation.Application;
using LibraryCirculation.Application.Exceptions;
using LibraryCirculation.Core;
using LibraryCirculation.Core.Common;
using LibraryCirculation.Core.Renting;
using LibraryCirculation.Core.Users.Readers;
using LibraryCirculation.WPF.Common;

namespace LibraryCirculation.WPF.LibrarianGUI.Reservations
{
    public partial class ReservationListingView
    {
        private readonly ReservationListingViewModel _viewModel;
        private readonly ReservationService _reservationService;

        public ReservationListingView()
        {
            InitializeComponent();
            _viewModel = new ReservationListingViewModel();
            _reservationService = Injector.GetService<ReservationService>();
            DataContext = _viewModel;
        }

        private void BtnApprove_Click(object sender, RoutedEventArgs e)
        {
            ManageReservation(true);
        }

        private void BtnDecline_Click(object sender, RoutedEventArgs e)
        {
            ManageReservation(false);
        }

        private void ManageReservation(bool approve)
        {
            try
            {
                Validate();
            }
            catch (ValidationException ve)
            {
                ViewUtil.ShowWarning(ve.Message);
                return;
            }

            var reservation = _reservationService.Get(GetSelectedId());
            reservation.Status = approve ? ReservationStatus.Approved : ReservationStatus.Declined;
            Injector.GetService<SystemUpdaterService>().TryAssignCopy(reservation);

            _reservationService.Update(reservation);
            _viewModel.LoadAll();
        }

        private void Validate(bool renting = false)
        {
            var id = GetSelectedId();
            if (id == 0)
                throw new ValidationException("Molimo izaberite rezervaciju.");

            var reservation = _reservationService.Get(id);

            switch (renting)
            {
                case false when reservation.Status != ReservationStatus.Submitted:
                    throw new ValidationException("Rezervacija za prihvatanje/odbijanje mora da bude na čekanju.");
                case false:
                    return;
            }

            if (reservation.Status != ReservationStatus.Approved)
                throw new ValidationException("Da bi se izdala knjiga za rezervaciju ona mora da bude prvo odobrena.");

            Injector.GetService<SystemUpdaterService>().TryAssignCopy(reservation);
            if (reservation.AssignedCopyId == 0)
                throw new ValidationException("Trenutno nema dostupnih slobodnih primeraka traženog naslova.");
        }

        private int GetSelectedId()
        {
            return _viewModel.SelectedReservation?.Id ?? 0;
        }

        private void BtnRent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Validate(true);
            }
            catch (ValidationException ve)
            {
                ViewUtil.ShowWarning(ve.Message);
                return;
            }

            var reservation = _reservationService.Get(GetSelectedId());
            var reader = Injector.GetService<ReaderService>().Get(reservation.ReaderUsername);
            var maxRentDays = Injector.GetService<MembershipService>().Get(reader.Membership).MaxRentDays;

            Injector.GetService<RentalService>().Add(new Rental(
                0, new TimeSlot(DateTime.Now, DateTime.Now.AddDays(maxRentDays)), 
                false, reservation.AssignedCopyId, reader.CardId));
            
            _reservationService.Remove(GetSelectedId());
            _viewModel.LoadAll();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}