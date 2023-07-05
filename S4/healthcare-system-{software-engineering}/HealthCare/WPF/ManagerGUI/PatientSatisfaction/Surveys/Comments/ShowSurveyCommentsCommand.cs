using System.Linq;
using HealthCare.Application;
using HealthCare.Core.PatientSatisfaction;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.ManagerGUI.PatientSatisfaction.Surveys.Comments
{
    internal class ShowSurveyCommentsCommand : CommandBase
    {
        private readonly SurveyListingViewModel _surveyListingModel;

        public ShowSurveyCommentsCommand(SurveyListingViewModel model)
        {
            _surveyListingModel = model;
        }

        public override void Execute(object parameter)
        {
            SurveyViewModel? surveyModel;

            if (_surveyListingModel is not null)
                surveyModel = _surveyListingModel.SelectedSurvey;
            else return;

            if (surveyModel is null) return;

            var surveys = Injector.GetService<SurveyService>()
                .GetForUser("")
                .Where(s => s.TopicName == surveyModel.Title && s.AdditionalComment != "")
                .ToList();

            new SurveyCommentsView(surveys).ShowDialog();
        }
    }
}