using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Users.Model;

namespace HealthCare.WPF.NurseGUI.PatientHealthcare.Referrals.Specialist
{
    public class SpecialistReferralViewModel
    {
        public SpecialistReferralViewModel(SpecialistReferral referral, Doctor from, Doctor to)
        {
            SpecialistReferral = referral;
            FromName = from.Name + " " + from.LastName;
            ToName = to.Name + " " + to.LastName;
            ReferredSpecialty = to.Specialization;
        }

        public SpecialistReferral SpecialistReferral { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string ReferredSpecialty { get; set; }
    }
}