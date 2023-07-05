using HealthCare.Core.Scheduling.Examination;

namespace HealthCare.Core.Scheduling.Availability
{
    public interface IAppointmentAvailable
    {
        bool IsAvailable(Appointment appointment);
    }
}