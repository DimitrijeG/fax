using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Examination.Command;

public class ResetEquipmentQuantityCommand : CommandBase
{
    private readonly UsedDynamicEquipmentViewModel _viewModel;

    public ResetEquipmentQuantityCommand(UsedDynamicEquipmentViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public override void Execute(object parameter)
    {
        _viewModel.Update();
    }
}