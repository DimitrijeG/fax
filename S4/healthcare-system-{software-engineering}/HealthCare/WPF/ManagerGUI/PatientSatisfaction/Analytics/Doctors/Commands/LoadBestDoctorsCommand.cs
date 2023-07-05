using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.ManagerGUI.PatientSatisfaction.Analytics.Doctors.Commands
{
    public class LoadBestDoctorsCommand : CommandBase
    {
        private readonly DoctorAnalyticsViewModel _model;

        public LoadBestDoctorsCommand(DoctorAnalyticsViewModel model)
        {
            _model = model;
        }

        public override void Execute(object parameter)
        {
            _model.LoadTop3();
        }
    }
}