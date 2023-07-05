using System.Collections.Generic;
using System.Windows;

namespace HealthCare.WPF.ManagerGUI.PatientSatisfaction.Surveys.Comments
{
    public partial class SurveyCommentsView : Window
    {
        public SurveyCommentsView(List<Core.PatientSatisfaction.Survey> surveys)
        {
            InitializeComponent();

            DataContext = new SurveyCommentsViewModel(surveys);
        }
    }
}