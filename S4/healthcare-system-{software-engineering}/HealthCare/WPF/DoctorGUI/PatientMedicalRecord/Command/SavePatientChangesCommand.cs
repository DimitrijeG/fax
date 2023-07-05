using System.Linq;
using HealthCare.Application;
using HealthCare.Application.Exceptions;
using HealthCare.Core.PatientHealthcare;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.PatientMedicalRecord.Command;

public class SavePatientChangesCommand : CommandBase
{
    private readonly PatientService _patientService;
    private readonly Patient _selectedPatient;
    private readonly PatientInformationViewModel _viewModel;

    public SavePatientChangesCommand(Patient patient, PatientInformationViewModel viewModel)
    {
        _patientService = Injector.GetService<PatientService>();
        _viewModel = viewModel;
        _selectedPatient = patient;
    }

    public override void Execute(object parameter)
    {
        try
        {
            Validate();

            UpdatePatientMedicalRecord();
            ShowSuccessMessage();
        }
        catch (ValidationException ve)
        {
            ViewUtil.ShowWarning(ve.Message);
        }
    }

    private void UpdatePatientMedicalRecord()
    {
        var weight = _viewModel.Weight;
        var height = _viewModel.Height;
        var medicalHistory = _viewModel.PreviousDisease.ToList();
        var allergies = _viewModel.Allergies.ToList();
        var updatedMedicalRecord = new MedicalRecord(height, weight, medicalHistory, allergies);

        _selectedPatient.MedicalRecord = updatedMedicalRecord;
        _patientService.Update(_selectedPatient);
    }

    private void ShowSuccessMessage()
    {
        ViewUtil.ShowInformation("Pacijent uspesno sacuvan!");
    }

    private void Validate()
    {
        if (_viewModel.Weight <= 0) throw new ValidationException("Neispravan unos tezine");
        if (_viewModel.Height <= 0) throw new ValidationException("Neispravan unos visine");
    }
}