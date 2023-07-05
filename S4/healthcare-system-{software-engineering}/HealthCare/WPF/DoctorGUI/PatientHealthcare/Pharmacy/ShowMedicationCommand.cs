using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Pharmacy;

public class ShowMedicationCommand : CommandBase
{
    private readonly Medication _medication;

    public ShowMedicationCommand(Medication medication)
    {
        _medication = medication;
    }

    public override void Execute(object parameter)
    {
        new MedicationInformationView(_medication).ShowDialog();
    }
}