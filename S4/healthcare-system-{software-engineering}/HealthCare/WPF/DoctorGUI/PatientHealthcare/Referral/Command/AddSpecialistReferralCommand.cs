using HealthCare.Application;
using HealthCare.Application.Exceptions;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Referral.Command;

public class AddSpecialistReferralCommand : CommandBase
{
    private readonly DoctorService _doctorService;
    private readonly SpecialistReferralService _specialistReferralService;
    private readonly SpecialistReferralViewModel _specialistReferralViewModel;

    public AddSpecialistReferralCommand(SpecialistReferralViewModel specialistReferralViewModel)
    {
        _specialistReferralService = Injector.GetService<SpecialistReferralService>();
        _doctorService = Injector.GetService<DoctorService>();
        _specialistReferralViewModel = specialistReferralViewModel;
    }

    public override void Execute(object parameter)
    {
        try
        {
            MakeReferral();
            ViewUtil.ShowInformation("Uput za doktora specijalistu uspesno dodat");
        }
        catch (ValidationException ex)
        {
            ViewUtil.ShowWarning(ex.Message);
        }
    }

    private void MakeReferral()
    {
        var doctorJMBG = Context.Current.JMBG;
        var patientJMBG = _specialistReferralViewModel.ExaminedPatient.JMBG;
        var isSpecializationReferral = _specialistReferralViewModel.IsSpecializationReferral;
        var referredDoctorJMBG = isSpecializationReferral ? SpecializationReferral() : DoctorReferral();

        var specialistReferral = new SpecialistReferral(patientJMBG, doctorJMBG, referredDoctorJMBG);
        _specialistReferralService.Add(specialistReferral);
    }

    private string SpecializationReferral()
    {
        var specialization = _specialistReferralViewModel.Specialization;
        if (_doctorService.GetFirstBySpecialization(specialization) is string jmbg) return jmbg;
        throw new ValidationException("Za datu specijalizaciju ne postoji trenutno doktor");
    }

    private string DoctorReferral()
    {
        if (_specialistReferralViewModel.SelectedDoctor is null)
            throw new ValidationException("Morate odabrati doktora iz tabele");

        return _specialistReferralViewModel.SelectedDoctor.JMBG;
    }
}