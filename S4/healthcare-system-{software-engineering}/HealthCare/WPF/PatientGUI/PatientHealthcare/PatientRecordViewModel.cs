using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using HealthCare.Application;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.NurseGUI.Scheduling;

namespace HealthCare.WPF.PatientGUI.PatientHealthcare
{
    public class PatientRecordViewModel
    {
        private readonly AnamnesisService _anamnesisService;
        private readonly AppointmentService _appointmentService;
        private readonly DoctorService _doctorService;
        private readonly PatientService _patientService;
        public List<Appointment> _patientAppointments;

        public PatientRecordViewModel()
        {
            _anamnesisService = Injector.GetService<AnamnesisService>();
            _doctorService = Injector.GetService<DoctorService>();
            _patientService = Injector.GetService<PatientService>();
            _appointmentService = Injector.GetService<AppointmentService>();
            Appointments = new ObservableCollection<AppointmentViewModel>();
            _patientAppointments = _appointmentService.GetByPatient(Context.Current.JMBG);
            LoadData(_patientAppointments);
        }

        public ObservableCollection<AppointmentViewModel> Appointments { get; set; }

        public void LoadData(List<Appointment> appointments)
        {
            Appointments.Clear();
            foreach (Appointment appointment in appointments)
            {
                Appointments.Add(new AppointmentViewModel(appointment, DateTime.Now));
            }
        }

        public void LoadData(List<AppointmentViewModel> appointments)
        {
            Appointments.Clear();
            foreach (AppointmentViewModel appointment in appointments)
            {
                Appointments.Add(appointment);
            }
        }

        public void Sort(string sortProperty)
        {
            switch (sortProperty)
            {
                case "Datum":
                    LoadData(Appointments.OrderBy(x => x.Appointment.TimeSlot.Start).ToList());
                    break;
                case "Doktor":
                    LoadData(Appointments.OrderBy(x => _doctorService.Get(x.Appointment.DoctorJMBG).Name).ToList());
                    break;
                case "Specijalizacija":
                    LoadData(Appointments.OrderBy(x => _doctorService.Get(x.Appointment.DoctorJMBG).Specialization)
                        .ToList());
                    break;
                default: break;
            }
        }

        public void Filter(string filterProperty)
        {
            IEnumerable<Appointment> query = _patientAppointments.ToList().Where(
                x =>
                    _doctorService.Get(x.DoctorJMBG).Name
                        .Contains(filterProperty, StringComparison.OrdinalIgnoreCase) ||
                    _doctorService.Get(x.DoctorJMBG).Specialization
                        .Contains(filterProperty, StringComparison.OrdinalIgnoreCase) ||
                    x.TimeSlot.Start.ToString().Contains(filterProperty, StringComparison.OrdinalIgnoreCase)
            ).ToList();
            LoadData(query.ToList());
        }

        public void ShowAnamnesis(Appointment appointment)
        {
            Anamnesis anamnesis;
            try
            {
                anamnesis = _anamnesisService.Get(appointment.AnamnesisID);
            }
            catch
            {
                MessageBox.Show("Pregled jos nije obavljen", "Anamneza");
                return;
            }

            Patient patient = _patientService.Get(appointment.PatientJMBG);
            Doctor doctor = _doctorService.Get(appointment.DoctorJMBG);
            string message = "Pacijent: " + patient.Name + " " + patient.LastName + "\n" +
                             "Doktor: " + doctor.Name + " " + doctor.LastName + "\n" +
                             "Simptomi: " + "\n";
            foreach (string symptom in anamnesis.Symptoms)
            {
                message += "   " + symptom + "\n";
            }

            message += "\n";
            message += "Zapazanja doktora: " + anamnesis.DoctorsObservations;
            MessageBox.Show(message, "Anamneza");
        }
    }
}