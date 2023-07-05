using System.Collections.ObjectModel;
using HealthCare.Application;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;

namespace HealthCare.WPF.NurseGUI.PatientHealthcare.Referrals.Specialist
{
    public class SpecialistReferralListingViewModel
    {
        private readonly DoctorService _doctorService;
        private readonly SpecialistReferralService _specialistReferralService;
        private Patient _patient;

        public SpecialistReferralListingViewModel(Patient patient)
        {
            Referrals = new ObservableCollection<SpecialistReferralViewModel>();
            _specialistReferralService = Injector.GetService<SpecialistReferralService>();
            _doctorService = Injector.GetService<DoctorService>();

            _patient = patient;

            Update();
        }

        public ObservableCollection<SpecialistReferralViewModel> Referrals { get; set; }

        public void Update()
        {
            Referrals.Clear();
            foreach (SpecialistReferral referral in _specialistReferralService.GetPatientsReferrals(_patient.JMBG))
                Referrals.Add(new SpecialistReferralViewModel(referral,
                    _doctorService.Get(referral.DoctorJMBG),
                    _doctorService.Get(referral.ReferredDoctorJMBG)));
        }
    }
}