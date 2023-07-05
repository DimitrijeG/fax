using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using HealthCare.Application;
using HealthCare.Application.Common;
using HealthCare.Core.Scheduling;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Scheduling.Schedules;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;
using HealthCare.WPF.NurseGUI.Scheduling;

namespace HealthCare.WPF.PatientGUI.Scheduling
{
    public partial class AppointmentCrudView
    {
        private readonly AppointmentService _appointmentService;
        private readonly DoctorService _doctorService;
        private readonly PatientService _patientService;
        private readonly Schedule _schedule;

        public AppointmentCrudView()
        {
            _schedule = Injector.GetService<Schedule>();
            _appointmentService = Injector.GetService<AppointmentService>();
            _patientService = Injector.GetService<PatientService>();
            _doctorService = Injector.GetService<DoctorService>();
            InitializeComponent();
            LoadData();
            IsUserBlocked();
        }

        public AppointmentCrudView(Doctor doctor)
        {
            _schedule = Injector.GetService<Schedule>();
            _appointmentService = Injector.GetService<AppointmentService>();
            _patientService = Injector.GetService<PatientService>();
            _doctorService = Injector.GetService<DoctorService>();
            InitializeComponent();
            LoadData();
            IsUserBlocked();
            List<Doctor> oneDoctor = new List<Doctor>
            {
                doctor
            };
            doctorListView.ItemsSource = new ObservableCollection<Doctor>(oneDoctor);
        }

        public void WriteActionToFile(string action)
        {
            string stringtocsv = Context.Current.JMBG + "|" + action + "|" + Util.ToString(DateTime.Now) +
                                 Environment.NewLine;
            File.AppendAllText(Paths.PATIENT_LOGS, stringtocsv);
        }

        public void IsUserBlocked()
        {
            Patient patient = (Patient)Context.Current;
            using (var reader = new StreamReader(Paths.PATIENT_LOGS, Encoding.Default))
            {
                string line;
                int updateDeleteCounter = 0;
                int createCounter = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split('|');
                    if (values[0] == patient.JMBG)
                    {
                        DateTime inputDate = Util.ParseDate(values[2]);
                        DateTime currentDate = DateTime.Now;
                        int daysDifference = (currentDate - inputDate).Days;
                        if (daysDifference < 30)
                        {
                            if (values[1] == "CREATE") createCounter++;
                            if (values[1] == "UPDATE" || values[1] == "DELETE") updateDeleteCounter++;
                        }
                    }
                }

                if (updateDeleteCounter >= 5 || createCounter > 8)
                {
                    patient.Blocked = true;
                }
                else
                {
                    patient.Blocked = false;
                }

                _patientService.Update(patient);
            }
        }

        public void LoadData()
        {
            List<Appointment> appointments = _appointmentService.GetByPatient(Context.Current.JMBG);
            List<Doctor> doctors = _doctorService.GetAll();
            var appModels = new ObservableCollection<AppointmentViewModel>();
            appointments.ForEach(a => appModels.Add(new AppointmentViewModel(a, DateTime.Now)));
            appListView.ItemsSource = appModels;
            doctorListView.ItemsSource = new ObservableCollection<Doctor>(doctors);
        }

        private void TbMinutes_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = tbMinutes.Text;
            if (int.TryParse(text, out int minutes))
            {
                if (minutes > 59 || minutes < 0)
                {
                    tbMinutes.Text = "0";
                }
            }
            else
            {
                tbMinutes.Text = "0";
            }
        }

        private void TbHours_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = tbHours.Text;
            if (int.TryParse(text, out int hours))
            {
                if (hours > 23 || hours < 0)
                {
                    tbHours.Text = "0";
                }
            }
            else
            {
                tbHours.Text = "0";
            }
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = (Patient)Context.Current;
            if (patient.Blocked)
            {
                ViewUtil.ShowWarning("Zao nam je, ali vas profil je blokiran");
                return;
            }

            Doctor doctor = (Doctor)doctorListView.SelectedItem;
            int hours = int.Parse(tbHours.Text);
            int minutes = int.Parse(tbMinutes.Text);
            if (!tbDate.SelectedDate.HasValue)
            {
                ViewUtil.ShowWarning("Molimo Vas izaberite datum");
                return;
            }
            else
            {
                DateTime currentDate = DateTime.Now;
                DateTime selectedDate = tbDate.SelectedDate.Value;
                selectedDate = selectedDate.AddHours(hours);
                selectedDate = selectedDate.AddMinutes(minutes);
                int difference = (selectedDate - currentDate).Days;
                if (difference < 1)
                {
                    ViewUtil.ShowWarning("Datum pregleda mora biti barem 1 dan od danasnjeg pregleda");
                    return;
                }
            }

            if (doctorListView.SelectedItems.Count != 1)
            {
                ViewUtil.ShowWarning("Molimo Vas izaberite doktora");
                return;
            }

            DateTime date = tbDate.SelectedDate.Value;
            date = date.AddHours(hours);
            date = date.AddMinutes(minutes);
            Appointment appointment = new Appointment(patient.JMBG, doctor.JMBG,
                new TimeSlot(date, new TimeSpan(0, 15, 0)), false);
            if (!_schedule.IsAvailable(appointment))
            {
                ViewUtil.ShowWarning("Doktor ili pacijent je zauzet u unetom terminu");
                return;
            }

            _schedule.Add(appointment);
            ViewUtil.ShowInformation("Uspesno dodat pregled");
            WriteActionToFile("CREATE");
            LoadData();
            IsUserBlocked();
        }

        private void AppListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (appListView.SelectedItems.Count == 1)
            {
                Appointment appointment = ((AppointmentViewModel)appListView.SelectedItem).Appointment;
                tbDate.SelectedDate = appointment.TimeSlot.Start;
                tbHours.Text = appointment.TimeSlot.Start.Hour.ToString();
                tbMinutes.Text = appointment.TimeSlot.Start.Minute.ToString();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = (Patient)Context.Current;
            if (patient.Blocked)
            {
                ViewUtil.ShowWarning("Zao nam je, ali vas profil je blokiran");
                return;
            }

            if (appListView.SelectedItems.Count == 1)
            {
                Appointment appointment = ((AppointmentViewModel)appListView.SelectedItem).Appointment;
                _appointmentService.Remove(appointment);
                WriteActionToFile("DELETE");
                ViewUtil.ShowInformation("Uspesno obrisan pregled");
                LoadData();
                IsUserBlocked();
            }
            else
            {
                ViewUtil.ShowWarning("Molimo Vas izaberite pregled");
                return;
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = (Patient)Context.Current;
            if (patient.Blocked)
            {
                ViewUtil.ShowWarning("Zao nam je, ali vas profil je blokiran");
                return;
            }

            Doctor doctor = (Doctor)doctorListView.SelectedItem;
            int hours = int.Parse(tbHours.Text);
            int minutes = int.Parse(tbMinutes.Text);
            if (!tbDate.SelectedDate.HasValue)
            {
                ViewUtil.ShowWarning("Molimo Vas izaberite datum");
                return;
            }
            else
            {
                DateTime currentDate = DateTime.Now;
                DateTime selectedDate = tbDate.SelectedDate.Value;
                selectedDate = selectedDate.AddHours(hours);
                selectedDate = selectedDate.AddMinutes(minutes);
                int difference = (selectedDate - currentDate).Days;
                if (difference < 1)
                {
                    ViewUtil.ShowWarning("Datum pregleda mora biti barem 1 dan od danasnjeg pregleda");
                    return;
                }
                else
                {
                }
            }

            if (doctorListView.SelectedItems.Count != 1)
            {
                ViewUtil.ShowWarning("Molimo Vas izaberite doktora");
                return;
            }

            DateTime date = tbDate.SelectedDate.Value;
            date = date.AddHours(hours);
            date = date.AddMinutes(minutes);
            if (appListView.SelectedItems.Count != 1)
            {
                ViewUtil.ShowWarning("Molimo Vas izaberite pregled");
                return;
            }

            Appointment appointment = new Appointment(patient.JMBG, doctor.JMBG,
                new TimeSlot(date, new TimeSpan(0, 15, 0)), false);
            Appointment appointment2 = ((AppointmentViewModel)appListView.SelectedItem).Appointment;
            appointment.AppointmentID = appointment2.AppointmentID;
            appointment.RoomID = appointment2.RoomID;
            if (!_schedule.IsAvailable(appointment))
            {
                ViewUtil.ShowWarning("Doktor ili pacijent je zauzet u unetom terminu");
                return;
            }

            _appointmentService.Update(appointment);
            ViewUtil.ShowInformation("Uspesno azuriran pregled");
            WriteActionToFile("UPDATE");
            LoadData();
            IsUserBlocked();
        }
    }
}