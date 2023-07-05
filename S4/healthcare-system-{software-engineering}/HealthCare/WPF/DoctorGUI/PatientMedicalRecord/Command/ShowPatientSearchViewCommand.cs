using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.PatientMedicalRecord.Command;

public class ShowPatientSearchViewCommand : CommandBase
{
    public override void Execute(object parameter)
    {
        new PatientSearchView().Show();
    }
}