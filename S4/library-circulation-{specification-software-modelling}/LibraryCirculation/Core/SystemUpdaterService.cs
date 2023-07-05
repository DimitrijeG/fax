using System;
using System.Linq;
using LibraryCirculation.Application;
using LibraryCirculation.Core.Fund;
using LibraryCirculation.Core.Renting;

namespace LibraryCirculation.Core
{
    public class SystemUpdaterService
    {
        private readonly ReservationService _reservationService;

        public SystemUpdaterService()
        {
            _reservationService = Injector.GetService<ReservationService>();
            UpdateData();
        }

        public void UpdateData()
        {
            UpdateReservationWaitingLists();
        }

        public void UpdateReservationWaitingLists()
        {
            _reservationService.GetWaitingList().ForEach(TryAssignCopy);
        }


        public void TryAssignCopy(Reservation reservation)
        {
            if (reservation.Status != ReservationStatus.Approved || reservation.AssignedCopyId != 0) return;

            var copyService = Injector.GetService<CopyService>();

            var reservedCopies = _reservationService.GetReservedCopyIds();
            var rentedCopies = Injector.GetService<RentalService>().GetRentedCopyIds();

            reservation.AssignedCopyId = copyService
                .GetForBook(reservation.BookIsbn)
                .Select(c => c.Id)
                .Where(id => !reservedCopies.Contains(id) && !rentedCopies.Contains(id))
                .FirstOrDefault(0);

            if (reservation.AssignedCopyId == 0) return;
            reservation.CopyAssignmentDate = DateTime.Now;

            _reservationService.Update(reservation);
        }
    }
}
