using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using HealthCare.Application;
using HealthCare.Core.PatientSatisfaction;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.PatientGUI.PatientSatisfaction
{
    public class SurveyDoctorViewModel
    {
        private ObservableCollection<Appointment> appointments;


        public AppointmentService appointmentService = Injector.GetService<AppointmentService>();

        private ObservableCollection<doctorRow> doctors;
        public DoctorService doctorService = Injector.GetService<DoctorService>();

        private Appointment selectedAppointment;

        private doctorRow selectedDoctor;
        private ObservableCollection<Survey> surveys;
        public SurveyService surveyService = Injector.GetService<SurveyService>();

        public SurveyDoctorViewModel()
        {
            Surveys = new ObservableCollection<Survey>();
            Doctors = new ObservableCollection<doctorRow>();
            loadSurveys();
            loadAppointments();
        }

        public ObservableCollection<Survey> Surveys
        {
            get => surveys;
            set
            {
                if (surveys != value)
                {
                    surveys = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Appointment> Appointments
        {
            get => appointments;
            set
            {
                if (appointments != value)
                {
                    appointments = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<doctorRow> Doctors
        {
            get => doctors;
            set
            {
                if (doctors != value)
                {
                    doctors = value;
                    OnPropertyChanged();
                }
            }
        }

        public doctorRow SelectedDoctor
        {
            get { return selectedDoctor; }
            set
            {
                selectedDoctor = value;
                OnPropertyChanged(nameof(SelectedDoctor));
            }
        }

        public Appointment SelectedAppointment
        {
            get { return selectedAppointment; }
            set { selectedAppointment = value; }
        }

        public RelayCommand SubmitSurvey { get; set; }


        public void loadSurveys()
        {
            Surveys.Add(new Survey
            {
                TopicName = "GENERALNO",
                DoctorJMBG = "",
                Description = "Kako ste zadovoljni generalnim kvalitetom usluge?",
                SelectedRating = 0,
                AdditionalComment = ""
            });


            Surveys.Add(new Survey
            {
                TopicName = "PREPORUKA",
                DoctorJMBG = "",
                Description = "Da li biste predlozili doktora svojim prijateljima?",
                SelectedRating = 0,
                AdditionalComment = ""
            });


            SubmitSurvey = new RelayCommand(o =>
            {
                if (selectedDoctor == null)
                {
                    ViewUtil.ShowWarning("Morate izabrati pregled");
                    return;
                }

                if (!checkAllSurveys())
                {
                    ViewUtil.ShowWarning("Morate popuniti sva polja");
                    return;
                }

                ViewUtil.ShowInformation("Uspesno ste popunili anketu");
                foreach (Survey survey in Surveys)
                {
                    Survey newSurvey = new Survey();
                    newSurvey.TopicName = survey.TopicName;
                    newSurvey.Description = survey.Description;
                    newSurvey.AdditionalComment = survey.AdditionalComment;
                    newSurvey.SelectedRating = survey.SelectedRating;
                    newSurvey.DoctorJMBG = SelectedDoctor.DoctorJMBG;
                    surveyService.Add(newSurvey);
                }

                double newRating = surveyService.GetAverageDoctor(SelectedDoctor.DoctorJMBG);
                Doctor doctor = doctorService.Get(SelectedDoctor.DoctorJMBG);
                doctor.Rating = newRating;
                doctorService.Update(doctor);
            });
        }

        public bool checkAllSurveys()
        {
            int unCheckedSurveys = Surveys.Count(m => m.SelectedRating == 0);
            if (unCheckedSurveys > 0) return false;
            return true;
        }

        public void loadAppointments()
        {
            Appointments = new ObservableCollection<Appointment>(appointmentService.GetByPatient(Context.Current.JMBG));
            foreach (Appointment appointment in Appointments)
            {
                Doctor doctor = doctorService.Get(appointment.DoctorJMBG);
                Doctors.Add(new doctorRow(doctor.JMBG, appointment.AppointmentID, appointment.TimeSlot.Start,
                    doctor.Name, doctor.LastName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class doctorRow
    {
        public doctorRow()
        {
        }

        public doctorRow(string doctorJMBG, int appointmentID, DateTime time, string name, string surname)
        {
            DoctorJMBG = doctorJMBG;
            AppointmentID = appointmentID;
            Time = time;
            Name = name;
            LastName = surname;
        }

        public string JMBG { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public int AppointmentID { get; set; }
        public DateTime Time { get; set; }
        public string DoctorJMBG { get; set; }
    }
}