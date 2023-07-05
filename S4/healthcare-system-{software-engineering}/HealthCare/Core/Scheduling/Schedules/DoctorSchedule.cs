using System;
using System.Collections.Generic;
using System.Linq;
using HealthCare.Application;
using HealthCare.Core.Scheduling.Availability;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Users.Model;

namespace HealthCare.Core.Scheduling.Schedules
{
    public class DoctorSchedule : ScheduleBase<string>, IAppointmentAvailable
    {
        private AppointmentService _appointmentService;

        public DoctorSchedule()
        {
            _appointmentService = Injector.GetService<AppointmentService>();
            _availabilityValidators = new List<IAvailable<string>>
            {
                new DoctorAppointmentAvailable(),
                new DoctorAbsenceRequestAvailable()
            };
        }

        public bool IsAvailable(Appointment appointment)
        {
            return IsAvailable(appointment.DoctorJMBG, appointment.TimeSlot);
        }

        public List<Appointment> GetAppointmentsForDays(Doctor doctor, DateTime start, int days)
        {
            DateTime end = start.AddDays(days);
            return _appointmentService
                .GetByDoctor(doctor.JMBG)
                .Where(x => x.TimeSlot.InBetweenDates(start, end))
                .ToList();
        }

        public List<Appointment> GetPostponable(TimeSpan duration, string doctorJmbg)
        {
            var postponable = _appointmentService.GetByDoctor(doctorJmbg)
                .Where(x => x.TimeSlot.Start > DateTime.Now && !x.IsUrgent).ToList();
            return FilterPostponable(duration, postponable);
        }

        private List<Appointment> FilterPostponable(TimeSpan duration, List<Appointment> postponable)
        {
            postponable = postponable.OrderBy(x => x.TimeSlot.Start).ToList();
            var filtered = new List<Appointment>();

            for (int i = 0; i < postponable.Count - 1; i++)
                if (postponable[i].TimeSlot.Start + duration <= postponable[i + 1].TimeSlot.Start)
                    filtered.Add(postponable[i]);

            if (postponable.Count > 0)
                filtered.Add(postponable.Last());

            return filtered;
        }
    }
}