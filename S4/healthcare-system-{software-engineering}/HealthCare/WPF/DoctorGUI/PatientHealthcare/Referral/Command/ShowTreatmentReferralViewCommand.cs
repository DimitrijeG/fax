using HealthCare.Core.Users.Model;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Referral.Command;

public class ShowTreatmentReferralViewCommand : CommandBase
{
    private readonly Patient _examinedPatient;

    public ShowTreatmentReferralViewCommand(Patient patient)
    {
        _examinedPatient = patient;
    }

    public override void Execute(object parameter)
    {
        new TreatmentReferralView(_examinedPatient).ShowDialog();
    }
}