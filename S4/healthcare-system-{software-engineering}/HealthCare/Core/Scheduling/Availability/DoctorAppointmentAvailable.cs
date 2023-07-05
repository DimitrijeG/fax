using System.Linq;
using HealthCare.Application;
using HealthCare.Core.Scheduling.Examination;

namespace HealthCare.Core.Scheduling.Availability
{
    public class DoctorAppointmentAvailable : IAvailable<string>
    {
        private readonly AppointmentService _appointmentService;

        public DoctorAppointmentAvailable()
        {
            _appointmentService = Injector.GetService<AppointmentService>();
        }

        public bool IsAvailable(string key, TimeSlot timeSlot)
        {
            return _appointmentService.GetAll()
                .Where(x => x.DoctorJMBG == key)
                .All(x => !x.TimeSlot.Overlaps(timeSlot));
        }
    }
}