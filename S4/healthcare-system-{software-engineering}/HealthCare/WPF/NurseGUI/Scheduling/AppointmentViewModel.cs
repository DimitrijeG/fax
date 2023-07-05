using System;
using HealthCare.Application;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;

namespace HealthCare.WPF.NurseGUI.Scheduling
{
    public class AppointmentViewModel
    {
        public AppointmentViewModel(Appointment appointment, DateTime time)
        {
            Appointment = appointment;
            RescheduleTime = time;
            Doctor = Injector.GetService<DoctorService>().Get(appointment.DoctorJMBG);
            Patient = Injector.GetService<PatientService>().Get(appointment.PatientJMBG);
        }

        public Appointment Appointment { get; set; }
        public DateTime RescheduleTime { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
    }
}