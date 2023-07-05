using System;
using System.Collections.Generic;
using HealthCare.Application;
using HealthCare.Core.Scheduling.Availability;
using HealthCare.Core.Scheduling.Examination;

namespace HealthCare.Core.Scheduling.Schedules
{
    public class PatientSchedule : ScheduleBase<string>, IAppointmentAvailable
    {
        private AppointmentService _appointmentService;

        public PatientSchedule()
        {
            _appointmentService = Injector.GetService<AppointmentService>();
            _availabilityValidators = new List<IAvailable<string>>
            {
                new PatientAppointmentAvailable()
            };
        }

        public bool IsAvailable(Appointment appointment)
        {
            return IsAvailable(appointment.PatientJMBG, appointment.TimeSlot);
        }

        public Appointment? TryGetReceptionAppointment(string patientJMBG)
        {
            TimeSlot reception = new TimeSlot(DateTime.Now, new TimeSpan(0, 15, 0));

            return _appointmentService
                .GetByPatient(patientJMBG)
                .Find(x => !x.IsOperation && reception.Contains(x.TimeSlot.Start));
        }
    }
}