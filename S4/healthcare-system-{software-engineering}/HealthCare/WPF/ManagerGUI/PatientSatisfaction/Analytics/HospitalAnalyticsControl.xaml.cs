using System.Windows.Input;
using HealthCare.WPF.ManagerGUI.PatientSatisfaction.Surveys;

namespace HealthCare.WPF.ManagerGUI.PatientSatisfaction.Analytics
{
    public partial class HospitalAnalyticsControl
    {
        private readonly SurveyListingViewModel _model;

        public HospitalAnalyticsControl(SurveyListingViewModel model)
        {
            InitializeComponent();

            _model = model;
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _model.ShowSurveyCommentsCommand.Execute(this);
        }
    }
}