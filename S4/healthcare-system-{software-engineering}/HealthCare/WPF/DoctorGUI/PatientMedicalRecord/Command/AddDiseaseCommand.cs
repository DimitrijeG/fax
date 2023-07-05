using HealthCare.Application.Exceptions;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.PatientMedicalRecord.Command;

public class AddDiseaseCommand : CommandBase
{
    private readonly PatientInformationViewModel _viewModel;

    public AddDiseaseCommand(PatientInformationViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public override void Execute(object parameter)
    {
        try
        {
            Validate();
            _viewModel.AddPreviousDisease(_viewModel.Disease);
        }
        catch (ValidationException ve)
        {
            ViewUtil.ShowWarning(ve.Message);
        }
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(_viewModel.Disease)) throw new ValidationException("Morate uneti bolest u polje");
    }
}