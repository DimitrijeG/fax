using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using HealthCare.Application;
using HealthCare.Core.PatientSatisfaction;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.PatientGUI.PatientSatisfaction
{
    public class SurveyHospitalViewModel
    {
        public AppointmentService appointmentService = Injector.GetService<AppointmentService>();
        private ObservableCollection<Survey> surveys;
        public SurveyService surveyService = Injector.GetService<SurveyService>();

        public SurveyHospitalViewModel()
        {
            Surveys = new ObservableCollection<Survey>();
            loadSurveys();
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

        public RelayCommand SubmitSurvey { get; set; }


        public void loadSurveys()
        {
            Surveys.Add(new Survey
            {
                TopicName = "GENERALNO",
                DoctorJMBG = "",
                Description = "Kako ste zadovoljni generalnim kvalitetom usluge bolince?",
                SelectedRating = 0,
                AdditionalComment = ""
            });

            Surveys.Add(new Survey
            {
                TopicName = "ČISTOĆA",
                DoctorJMBG = "",
                Description = "Kako ste zadovoljni cistocom bolnice?",
                SelectedRating = 0,
                AdditionalComment = ""
            });

            Surveys.Add(new Survey
            {
                TopicName = "PREPORUKA",
                DoctorJMBG = "",
                Description = "Da li biste predlozili bolnicu svojim prijateljima?",
                SelectedRating = 0,
                AdditionalComment = ""
            });


            SubmitSurvey = new RelayCommand(o =>
            {
                if (!checkAllSurveys())
                {
                    ViewUtil.ShowWarning("Niste popunili anketu");
                }
                else
                {
                    ViewUtil.ShowInformation("Uspesno ste popunili anketu");
                    foreach (Survey survey in Surveys)
                    {
                        Survey newSurvey = new Survey();
                        newSurvey.TopicName = survey.TopicName;
                        newSurvey.Description = survey.Description;
                        newSurvey.AdditionalComment = survey.AdditionalComment;
                        newSurvey.SelectedRating = survey.SelectedRating;
                        newSurvey.DoctorJMBG = survey.DoctorJMBG;
                        surveyService.Add(newSurvey);
                    }
                }
            });
        }

        public bool checkAllSurveys()
        {
            int unCheckedSurveys = Surveys.Count(m => m.SelectedRating == 0);
            if (unCheckedSurveys > 0) return false;
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}