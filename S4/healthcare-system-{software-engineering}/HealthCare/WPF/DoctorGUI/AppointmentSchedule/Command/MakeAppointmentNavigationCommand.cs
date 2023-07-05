using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.AppointmentSchedule.Command;

public class MakeAppointmentNavigationCommand : CommandBase
{
    private readonly DoctorMainViewModel _viewModel;

    public MakeAppointmentNavigationCommand(DoctorMainViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public override void Execute(object parameter)
    {
        new MakeAppointmentView(_viewModel).ShowDialog();
    }
}