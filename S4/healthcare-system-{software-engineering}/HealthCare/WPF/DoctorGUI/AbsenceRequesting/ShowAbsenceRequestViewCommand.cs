using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.AbsenceRequesting
{
    public class ShowAbsenceRequestViewCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            new AbsenceRequestView().ShowDialog();
        }
    }
}