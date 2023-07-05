using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using HealthCare.Application;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.ManagerGUI.PatientSatisfaction.Analytics.Doctors.Commands;
using HealthCare.WPF.ManagerGUI.PatientSatisfaction.Surveys.Comments;

namespace HealthCare.WPF.ManagerGUI.PatientSatisfaction.Analytics.Doctors
{
    public class DoctorAnalyticsViewModel
    {
        private readonly DoctorService _doctorService;

        public DoctorAnalyticsViewModel()
        {
            _doctorService = Injector.GetService<DoctorService>();
            ShowDoctorCommentsCommand = new ShowDoctorComments(this);
            LoadBestDoctorsCommand = new LoadBestDoctorsCommand(this);
            LoadWorstDoctorsCommand = new LoadWorstDoctorsCommand(this);
            LoadAllDoctorsCommand = new LoadAllDoctorsCommand(this);
            SurveyItems = new ObservableCollection<DoctorSurveyViewModel>();
            LoadAll();
        }

        public ObservableCollection<DoctorSurveyViewModel> SurveyItems { get; }
        public DoctorSurveyViewModel? SelectedDoctor { get; set; }
        public ICommand ShowDoctorCommentsCommand { get; }
        public ICommand LoadBestDoctorsCommand { get; }
        public ICommand LoadWorstDoctorsCommand { get; }
        public ICommand LoadAllDoctorsCommand { get; }

        public void LoadAll()
        {
            SurveyItems.Clear();
            _doctorService.GetAll()
                .ForEach(d => SurveyItems.Add(new DoctorSurveyViewModel(d)));
        }

        public void LoadTop3()
        {
            SurveyItems.Clear();
            _doctorService.GetAll()
                .Select(d => new DoctorSurveyViewModel(d))
                .OrderByDescending(m => m.Surveys.Average(s => s.Rating))
                .ThenBy(m => m.Doctor)
                .Take(3).ToList().ForEach(m => SurveyItems.Add(m));
        }

        public void LoadBottom3()
        {
            SurveyItems.Clear();
            _doctorService.GetAll()
                .Select(d => new DoctorSurveyViewModel(d))
                .OrderBy(m => m.Surveys.Average(s => s.Rating))
                .ThenBy(m => m.Doctor)
                .Take(3).ToList().ForEach(m => SurveyItems.Add(m));
        }
    }
}