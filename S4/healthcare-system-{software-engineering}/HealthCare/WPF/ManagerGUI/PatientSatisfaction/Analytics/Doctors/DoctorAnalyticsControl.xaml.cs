using System.Windows.Input;

namespace HealthCare.WPF.ManagerGUI.PatientSatisfaction.Analytics.Doctors
{
    public partial class DoctorAnalyticsControl
    {
        private readonly DoctorAnalyticsViewModel _model;

        public DoctorAnalyticsControl(DoctorAnalyticsViewModel model)
        {
            InitializeComponent();
            _model = model;
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _model.ShowDoctorCommentsCommand.Execute(this);
        }
    }
}