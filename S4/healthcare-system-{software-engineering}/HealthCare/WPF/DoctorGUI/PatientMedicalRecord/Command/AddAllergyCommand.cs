using HealthCare.Application.Exceptions;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.PatientMedicalRecord.Command;

public class AddAllergyCommand : CommandBase
{
    private readonly PatientInformationViewModel _viewModel;

    public AddAllergyCommand(PatientInformationViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public override void Execute(object parameter)
    {
        try
        {
            Validate();
            _viewModel.AddAllergy(_viewModel.Allergy);
        }
        catch (ValidationException ve)
        {
            ViewUtil.ShowWarning(ve.Message);
        }
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(_viewModel.Allergy))
            throw new ValidationException("Morate uneti alergiju u polje");
    }
}