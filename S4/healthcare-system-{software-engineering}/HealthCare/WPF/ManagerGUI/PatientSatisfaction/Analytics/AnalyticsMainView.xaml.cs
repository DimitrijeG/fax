using System.Windows;

namespace HealthCare.WPF.ManagerGUI.PatientSatisfaction.Analytics
{
    public partial class AnalyticsMainView : Window
    {
        public AnalyticsMainView()
        {
            InitializeComponent();

            DataContext = new SurveyAnalyticsViewModel();
        }
    }
}