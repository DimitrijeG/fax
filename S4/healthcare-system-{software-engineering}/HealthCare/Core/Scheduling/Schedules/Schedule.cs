using System;
using System.Collections.Generic;
using System.Linq;
using HealthCare.Application;
using HealthCare.Core.Scheduling.Availability;
using HealthCare.Core.Scheduling.Examination;

namespace HealthCare.Core.Scheduling.Schedules
{
    public class Schedule
    {
        private readonly AppointmentService _appointmentService;
        private readonly DoctorSchedule _doctorSchedule;
        private readonly RoomSchedule _roomSchedule;
        private List<IAppointmentAvailable> _schedules;

        public Schedule()
        {
            _appointmentService = Injector.GetService<AppointmentService>();
            _doctorSchedule = Injector.GetService<DoctorSchedule>();
            _roomSchedule = Injector.GetService<RoomSchedule>();
            _schedules = new List<IAppointmentAvailable>
            {
                Injector.GetService<PatientSchedule>(),
                _doctorSchedule,
                _roomSchedule
            };
        }

        public bool IsAvailable(Appointment appointment)
        {
            return _schedules.All(x => x.IsAvailable(appointment));
        }

        public TimeSlot GetSoonestTimeSlot(Appointment given)
        {
            DateTime end, postpone = DateTime.MaxValue;
            TimeSpan duration = given.TimeSlot.Duration;
            TimeSlot temp = given.TimeSlot;

            foreach (Appointment appointment in _appointmentService.GetAll())
            {
                end = appointment.TimeSlot.End;
                given.TimeSlot = new TimeSlot(end, duration);

                if (end >= DateTime.Now && end < postpone && IsAvailable(given))
                    postpone = end;
            }

            given.TimeSlot = temp;
            return new TimeSlot(postpone, duration);
        }

        public void Postpone(Appointment appointment)
        {
            appointment.TimeSlot = GetSoonestTimeSlot(appointment);
            _appointmentService.Update(appointment);
        }

        public void AddUrgent(Appointment appointment)
        {
            _appointmentService.GetOverlapping(appointment).ForEach(a => Postpone(a));

            appointment.IsUrgent = true;
            Add(appointment);
        }

        public void Add(Appointment appointment)
        {
            _roomSchedule.SetFirstAvailableRoom(appointment);
            _appointmentService.Add(appointment);
        }

        public Appointment? TryGetUrgent(TimeSpan duration, List<string> specialists)
        {
            var possible = specialists.Select(doctor => GetUrgentForDoctor(duration, doctor));

            Appointment? best = possible.OrderBy(x => x.TimeSlot.Start).FirstOrDefault();

            DateTime twoHours = DateTime.Now + new TimeSpan(2, 0, 0);
            return best is not null && best.TimeSlot.Start <= twoHours ? best : null;
        }

        private Appointment GetUrgentForDoctor(TimeSpan duration, string doctorJMBG)
        {
            Appointment urgent = new Appointment();
            urgent.DoctorJMBG = doctorJMBG;
            urgent.TimeSlot = new TimeSlot(DateTime.Now, duration);

            if (_doctorSchedule.IsAvailable(urgent))
                return urgent;

            urgent.TimeSlot.Start = DateTime.MaxValue;
            foreach (Appointment appointment in _appointmentService.GetByDoctor(doctorJMBG))
            {
                DateTime end = appointment.TimeSlot.End;
                TimeSlot slot = new TimeSlot(end, duration);

                if (end > DateTime.Now && end < urgent.TimeSlot.Start &&
                    _doctorSchedule.IsAvailable(doctorJMBG, slot) && !appointment.IsUrgent)
                    urgent.TimeSlot.Start = end;
            }

            return urgent;
        }
    }
}