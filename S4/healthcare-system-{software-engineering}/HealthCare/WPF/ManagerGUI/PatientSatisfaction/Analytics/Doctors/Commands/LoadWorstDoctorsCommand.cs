using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.ManagerGUI.PatientSatisfaction.Analytics.Doctors.Commands
{
    public class LoadWorstDoctorsCommand : CommandBase
    {
        private readonly DoctorAnalyticsViewModel _model;

        public LoadWorstDoctorsCommand(DoctorAnalyticsViewModel model)
        {
            _model = model;
        }

        public override void Execute(object parameter)
        {
            _model.LoadBottom3();
        }
    }
}