using HealthCare.Core.Users.Model;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Referral.Command;

public class ShowSpecialistReferralViewCommand : CommandBase
{
    private readonly Patient _examinedPatient;

    public ShowSpecialistReferralViewCommand(Patient patient)
    {
        _examinedPatient = patient;
    }

    public override void Execute(object parameter)
    {
        new SpecialistReferralView(_examinedPatient).ShowDialog();
    }
}