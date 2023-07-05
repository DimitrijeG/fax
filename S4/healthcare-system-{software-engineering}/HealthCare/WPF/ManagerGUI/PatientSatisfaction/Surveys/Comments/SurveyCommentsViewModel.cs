using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HealthCare.Core.PatientSatisfaction;

namespace HealthCare.WPF.ManagerGUI.PatientSatisfaction.Surveys.Comments
{
    public class SurveyCommentsViewModel
    {
        public SurveyCommentsViewModel(List<Survey> surveys)
        {
            Surveys = new ObservableCollection<Survey>();
            surveys
                .OrderByDescending(s => s.SelectedRating)
                .ThenBy(s => s.surveyID)
                .ToList().ForEach(s => Surveys.Add(s));
        }

        public ObservableCollection<Survey> Surveys { get; }
    }
}