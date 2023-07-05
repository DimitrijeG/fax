using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HealthCare.Application;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Scheduling.Schedules;

namespace HealthCare.WPF.NurseGUI.Scheduling
{
    public class PostponableAppointmentsViewModel
    {
        public PostponableAppointmentsViewModel(List<Appointment> postponable)
        {
            Schedule _schedule = Injector.GetService<Schedule>();
            Appointments = new ObservableCollection<AppointmentViewModel>();

            for (int i = 0; i < Math.Min(5, postponable.Count); i++)
                Appointments.Add(new AppointmentViewModel(
                    postponable[i],
                    _schedule.GetSoonestTimeSlot(postponable[i]).Start));
        }

        public ObservableCollection<AppointmentViewModel> Appointments { get; set; }
    }
}