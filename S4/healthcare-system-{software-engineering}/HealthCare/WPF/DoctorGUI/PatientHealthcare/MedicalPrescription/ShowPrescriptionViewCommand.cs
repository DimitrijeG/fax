using HealthCare.Core.Users.Model;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicalPrescription;

public class ShowPrescriptionViewCommand : CommandBase
{
    private readonly Patient _patient;

    public ShowPrescriptionViewCommand(Patient patient)
    {
        _patient = patient;
    }

    public override void Execute(object parameter)
    {
        new PrescriptionView(_patient).ShowDialog();
    }
}