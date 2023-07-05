using System.Linq;
using HealthCare.Application;
using HealthCare.Core.PatientSatisfaction;
using HealthCare.WPF.Common.Command;
using HealthCare.WPF.ManagerGUI.PatientSatisfaction.Analytics.Doctors;

namespace HealthCare.WPF.ManagerGUI.PatientSatisfaction.Surveys.Comments
{
    public class ShowDoctorComments : CommandBase
    {
        private readonly DoctorAnalyticsViewModel _surveyListingModel;

        public ShowDoctorComments(DoctorAnalyticsViewModel model)
        {
            _surveyListingModel = model;
        }

        public override void Execute(object parameter)
        {
            DoctorSurveyViewModel? surveyModel;

            if (_surveyListingModel is not null)
                surveyModel = _surveyListingModel.SelectedDoctor;
            else return;

            if (surveyModel is null) return;

            var surveys = Injector.GetService<SurveyService>()
                .GetForUser(surveyModel.Jmbg)
                .Where(s => s.AdditionalComment != "")
                .ToList();

            new SurveyCommentsView(surveys).ShowDialog();
        }
    }
}