using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using HealthCare.Application;
using HealthCare.Core.Notifications;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Scheduling.Schedules;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.NurseGUI.Scheduling
{
    public partial class PostponableAppointmentsView : Window
    {
        private readonly NotificationService _notificationService;
        private readonly Schedule _schedule;
        private PostponableAppointmentsViewModel _model;
        private Appointment _newAppointment;
        private AppointmentViewModel? _selected;

        public PostponableAppointmentsView(Appointment newAppointment, List<Appointment> postponable)
        {
            InitializeComponent();
            _newAppointment = newAppointment;

            _notificationService = Injector.GetService<NotificationService>();
            _schedule = Injector.GetService<Schedule>();

            _model = new PostponableAppointmentsViewModel(postponable);
            DataContext = _model;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void lvAppointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selected = (AppointmentViewModel)lvAppointments.SelectedItem;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (_selected is null)
            {
                ViewUtil.ShowWarning("Izaberite polje iz tabele");
                return;
            }

            _newAppointment.TimeSlot.Start = _selected.Appointment.TimeSlot.Start;
            _newAppointment.DoctorJMBG = _selected.Appointment.DoctorJMBG;
            _schedule.Postpone(_selected.Appointment);
            _schedule.AddUrgent(_newAppointment);

            _notificationService.Add(new Notification(
                $"Termin sa ID-jem {_selected.Appointment.AppointmentID} je pomeren.",
                _selected.Appointment.DoctorJMBG, _selected.Appointment.PatientJMBG));
            _notificationService.Add(new Notification(
                $"Hitan termin sa ID-jem {_newAppointment.AppointmentID} je kreiran.",
                _selected.Appointment.DoctorJMBG));

            ViewUtil.ShowInformation("Uspesno odlozen termin.");
            Close();
        }
    }
}