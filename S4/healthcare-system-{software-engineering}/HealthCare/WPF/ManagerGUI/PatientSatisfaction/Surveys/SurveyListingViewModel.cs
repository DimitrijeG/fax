using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using HealthCare.Application;
using HealthCare.Core.PatientSatisfaction;
using HealthCare.WPF.ManagerGUI.PatientSatisfaction.Surveys.Comments;

namespace HealthCare.WPF.ManagerGUI.PatientSatisfaction.Surveys
{
    public class SurveyListingViewModel
    {
        public SurveyListingViewModel()
        {
            ShowSurveyCommentsCommand = new ShowSurveyCommentsCommand(this);
            SurveyItems = new ObservableCollection<SurveyViewModel>();
            LoadAll();
        }

        public ObservableCollection<SurveyViewModel> SurveyItems { get; }

        public SurveyViewModel? SelectedSurvey { get; set; }
        public ICommand ShowSurveyCommentsCommand { get; }

        private void LoadAll()
        {
            SurveyItems.Clear();

            var service = Injector.GetService<SurveyService>();
            service.GetHospitalSurveys()
                .GroupBy(s => s.TopicName)
                .Select(g => new SurveyViewModel(
                        g.Key,
                        g.Average(x => x.SelectedRating ?? 0),
                        new List<int>
                        {
                            g.Count(x => x.SelectedRating == 1),
                            g.Count(x => x.SelectedRating == 2),
                            g.Count(x => x.SelectedRating == 3),
                            g.Count(x => x.SelectedRating == 4),
                            g.Count(x => x.SelectedRating == 5)
                        }
                    )
                ).ToList().ForEach(m => SurveyItems.Add(m));
        }
    }
}