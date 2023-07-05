using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using HealthCare.Application;
using HealthCare.Application.Common;
using HealthCare.Core.Scheduling;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Scheduling.Schedules;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.PatientGUI.Scheduling
{
    public class CreatePriorityAppointmentViewModel
    {
        public CreatePriorityAppointmentViewModel()
        {
            appointmentService = Injector.GetService<AppointmentService>();
            doctorService = Injector.GetService<DoctorService>();
            patientService = Injector.GetService<PatientService>();
            Doctors = doctorService.GetAll();
            schedule = Injector.GetService<Schedule>();
            ResultAppointments = new ObservableCollection<Appointment>();
            EndDate = DateTime.Now;

            ShowAppointmentCommand = new RelayCommand(o => { CalculateAppointments(); });

            CreateAppointmentCommand = new RelayCommand(o => { CreateAppointment(); });

            ChangePriorityCommand = new RelayCommand(o =>
            {
                string priority = o as string;
                ChangePriority(priority);
            });
        }

        private AppointmentService appointmentService { get; set; }

        private Schedule schedule { get; set; }

        private DoctorService doctorService { get; set; }

        private PatientService patientService { get; set; }
        public List<Doctor> Doctors { get; set; }
        public Doctor SelectedDoctor { get; set; }
        public DateTime EndDate { get; set; }
        public int HourStart { get; set; }

        public int HourEnd { get; set; }
        public int MinuteStart { get; set; }
        public int MinuteEnd { get; set; }
        public bool IsDoctorPriority { get; set; }

        public ObservableCollection<Appointment> resultAppointments { get; set; }

        public ObservableCollection<Appointment> ResultAppointments
        {
            get => resultAppointments;
            set
            {
                resultAppointments = value;
                OnPropertyChanged(nameof(ResultAppointments));
            }
        }

        public Appointment SelectedAppointment { get; set; }
        public RelayCommand ShowAppointmentCommand { get; set; }
        public RelayCommand ChangePriorityCommand { get; set; }
        public RelayCommand CreateAppointmentCommand { get; set; }

        public void ChangePriority(string priority)
        {
            if (priority.Equals("Doctor"))
            {
                IsDoctorPriority = true;
                return;
            }

            IsDoctorPriority = false;
            return;
        }


        public bool ValidateAllData()
        {
            Patient patient = (Patient)Context.Current;
            if (patient.Blocked)
                return false;

            if (EndDate == null || HourStart == null || MinuteStart == null || MinuteStart == null ||
                MinuteEnd == null || SelectedDoctor == null)
                return false;
            else
            {
                DateTime currentDate = DateTime.Now;
                DateTime selectedDate = EndDate;
                selectedDate = selectedDate.AddHours(HourStart);
                selectedDate = selectedDate.AddMinutes(MinuteStart);
                if (selectedDate < currentDate) return false;
            }

            if (HourStart > HourEnd || HourStart == HourEnd && MinuteStart >= MinuteEnd) return false;

            return true;
        }

        public void LoadAppointments(List<Appointment> appointments)
        {
            ResultAppointments.Clear();
            List<Appointment> list = new List<Appointment>();
            foreach (Appointment appointment in appointments)
            {
                ResultAppointments.Add(appointment);
            }
        }

        public void SelectAppointment(int selectedID)
        {
            Appointment appointment = ResultAppointments.Where(x => x.AppointmentID == selectedID).First();
            if (appointment == null) return;
            SelectedAppointment = appointment;
        }

        public void CalculateAppointments()
        {
            if (!ValidateAllData()) return;
            string priority;
            if (IsDoctorPriority) priority = "Doctor";
            else priority = "Date";
            GetAppointments(EndDate, HourStart, MinuteStart, HourEnd, MinuteEnd, SelectedDoctor, priority);
        }

        public void CreateAppointment()
        {
            if (SelectedAppointment == null) return;
            if (!schedule.IsAvailable(SelectedAppointment)) return;
            schedule.Add(SelectedAppointment);
            WriteAction("CREATE");
            IsUserBlocked();
        }

        public void GetAppointments(DateTime endDate, int hoursStart, int minutesStart, int hoursEnd, int minutesEnd,
            Doctor doctor, string priority)
        {
            endDate = endDate.AddHours(hoursEnd);
            endDate = endDate.AddMinutes(minutesEnd);
            Appointment resultAppointment;

            if (priority == "Date")
            {
                resultAppointment =
                    GetAppointmentByDateAndDoctor(endDate, hoursStart, minutesStart, hoursEnd, minutesEnd, doctor);
                if (resultAppointment == null)
                {
                    resultAppointment = GetAppointmentByDate(endDate, hoursStart, minutesStart, hoursEnd, minutesEnd);
                }
            }
            else
            {
                resultAppointment =
                    GetAppointmentByDoctor(endDate, hoursStart, minutesStart, hoursEnd, minutesEnd, doctor);
                if (resultAppointment == null)
                {
                    resultAppointment = GetAppointmentByDateAndDoctor(endDate, hoursStart, minutesStart, hoursEnd,
                        minutesEnd, doctor);
                }
            }

            LoadAppointments(resultAppointment != null
                ? new List<Appointment> { resultAppointment }
                : GetAppointmentByDoctor(doctor));
        }

        public Appointment GetAppointmentByDoctor(DateTime endDate, int hoursStart, int minutesStart, int hoursEnd,
            int minutesEnd, Doctor doctor)
        {
            endDate = endDate.AddHours(hoursEnd);
            endDate = endDate.AddMinutes(minutesEnd);
            DateTime startDate = DateTime.Today.AddMinutes(15);
            Patient patient = (Patient)Context.Current;

            while (startDate < endDate)
            {
                TimeSlot timeSlot = new TimeSlot(startDate, new TimeSpan(0, 15, 0));
                Appointment appointment = new Appointment(patient.JMBG, doctor.JMBG, timeSlot, false);

                if (schedule.IsAvailable(appointment))
                {
                    return appointment;
                }
                else
                {
                    startDate = startDate.AddMinutes(15);

                    if (startDate.Hour >= hoursEnd && startDate.Minute >= minutesEnd)
                    {
                        startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day + 1, hoursStart,
                            minutesStart, 0);
                    }
                }
            }

            return null;
        }

        public List<Appointment> GetAppointmentByDoctor(Doctor doctor)
        {
            DateTime startDate = DateTime.Today;
            startDate = startDate.AddMinutes(15);
            List<Appointment> appointments = new List<Appointment>();
            Patient patient = (Patient)Context.Current;
            while (appointments.Count() < 3)
            {
                TimeSlot timeSlot = new TimeSlot(startDate, new TimeSpan(0, 15, 0));
                Appointment appointment = new Appointment(patient.JMBG, doctor.JMBG, timeSlot, false);
                if (schedule.IsAvailable(appointment))
                {
                    appointments.Add(appointment);
                }

                startDate = startDate.AddMinutes(15);
            }

            return appointments;
        }

        public Appointment GetAppointmentByDate(DateTime endDate, int hoursStart, int minutesStart, int hoursEnd,
            int minutesEnd)
        {
            endDate = endDate.AddHours(hoursEnd);
            endDate = endDate.AddMinutes(minutesEnd);
            Patient patient = (Patient)Context.Current;
            List<Doctor> doctors = doctorService.GetAll();

            foreach (Doctor doctor in doctors)
            {
                DateTime startDate = DateTime.Today.AddHours(hoursStart).AddMinutes(minutesStart);

                while (startDate < endDate)
                {
                    TimeSlot timeSlot = new TimeSlot(startDate, new TimeSpan(0, 15, 0));
                    Appointment appointment = new Appointment(patient.JMBG, doctor.JMBG, timeSlot, false);

                    if (schedule.IsAvailable(appointment))
                    {
                        return appointment;
                    }
                    else
                    {
                        startDate = startDate.AddMinutes(15);

                        if (startDate.Hour >= hoursEnd && startDate.Minute >= minutesEnd)
                        {
                            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day + 1, hoursStart,
                                minutesStart, 0);
                        }
                    }
                }
            }

            return null;
        }

        public Appointment GetAppointmentByDateAndDoctor(DateTime endDate, int hoursStart, int minutesStart,
            int hoursEnd, int minutesEnd, Doctor doctor)
        {
            endDate = endDate.AddHours(hoursEnd);
            endDate = endDate.AddMinutes(minutesEnd);
            DateTime startDate = DateTime.Today.AddHours(hoursStart).AddMinutes(minutesStart);
            Patient patient = (Patient)Context.Current;

            while (startDate < endDate)
            {
                TimeSlot timeSlot = new TimeSlot(startDate, new TimeSpan(0, 15, 0));
                Appointment appointment = new Appointment(patient.JMBG, doctor.JMBG, timeSlot, false);

                if (schedule.IsAvailable(appointment))
                {
                    return appointment;
                }
                else
                {
                    startDate = startDate.AddMinutes(15);

                    if (startDate.Hour >= hoursEnd && startDate.Minute >= minutesEnd)
                    {
                        startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day + 1, hoursStart,
                            minutesStart, 0);
                    }
                }
            }

            return null;
        }

        public void WriteAction(string action)
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

                if (updateDeleteCounter >= 5 || createCounter > 8) patient.Blocked = true;
                else patient.Blocked = false;

                patientService.Update(patient);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}