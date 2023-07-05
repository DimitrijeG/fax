using System;
using System.Collections.Generic;
using System.Linq;
using LibraryCirculation.Application;
using LibraryCirculation.Core.Common;
using LibraryCirculation.DataManagement.Repository;

namespace LibraryCirculation.Core.Renting
{
    public class ReservationService : CrudService<Reservation>
    {
        public ReservationService(IRepository<Reservation> repository) : base(repository)
        {
            UpdateReservationStatuses();
        }

        public List<Reservation> GetApproved()
        {
            return GetAll().Where(r => r.Status == ReservationStatus.Approved).ToList();
        }
        public List<Reservation> GetWaitingList()
        {
            return GetApproved()
                .Where(r => r.AssignedCopyId == 0)
                .OrderBy(r => r.SubmittingDate).ToList();
        }

        public List<int> GetReservedCopyIds()
        {
            return GetAll().Select(r => r.AssignedCopyId).Where(id => id != 0).ToList();
        }

        public void UpdateReservationStatuses()
        {
            GetApproved()
                .Where(r => r.AssignedCopyId != 0 && r.CopyAssignmentDate.AddDays(2) < DateTime.Now)
                .ToList().ForEach(r =>
                {
                    r.Status = ReservationStatus.Declined;
                    r.AssignedCopyId = 0;
                    Update(r);
                });
        }
    }
}