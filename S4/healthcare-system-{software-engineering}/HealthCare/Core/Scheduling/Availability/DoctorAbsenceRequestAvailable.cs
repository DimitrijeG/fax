using System.Linq;
using HealthCare.Application;
using HealthCare.Core.HumanResources;

namespace HealthCare.Core.Scheduling.Availability
{
    public class DoctorAbsenceRequestAvailable : IAvailable<string>
    {
        private readonly AbsenceRequestService _absenceRequestService;

        public DoctorAbsenceRequestAvailable()
        {
            _absenceRequestService = Injector.GetService<AbsenceRequestService>();
        }

        public bool IsAvailable(string key, TimeSlot timeSlot)
        {
            return _absenceRequestService.GetAll()
                .Where(x => x.RequesterJMBG == key && x.IsApproved)
                .All(x => !x.AbsenceDuration.Overlaps(timeSlot));
        }
    }
}