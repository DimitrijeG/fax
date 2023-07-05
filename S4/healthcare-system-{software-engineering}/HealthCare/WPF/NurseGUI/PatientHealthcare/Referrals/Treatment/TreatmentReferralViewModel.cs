using System.Linq;
using HealthCare.Application;
using HealthCare.Core.PatientHealthcare.HealthcareTreatment;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.Core.Users.Service;

namespace HealthCare.WPF.NurseGUI.PatientHealthcare.Referrals.Treatment
{
    public class TreatmentReferralViewModel
    {
        public TreatmentReferralViewModel(TreatmentReferral referral)
        {
            var doctorService = Injector.GetService<DoctorService>();
            var therapyService = Injector.GetService<TherapyService>();
            var medicationService = Injector.GetService<MedicationService>();
            var prescriptionService = Injector.GetService<PrescriptionService>(Injector.THERAPY_PRESCRIPTION_S);

            var doctor = doctorService.Get(referral.DoctorJMBG);

            Id = referral.Id;
            Days = referral.DaysOfTreatment;
            Doctor = doctor.Name + " " + doctor.LastName;
            var medications = therapyService.Get(referral.TherapyID)
                .InitialMedication.Select(id => prescriptionService.Get(id))
                .Select(t => medicationService.Get(t.MedicationId).Name);
            Therapy = string.Join(",", medications);
        }

        public int Id { get; set; }
        public int Days { get; set; }
        public string Doctor { get; set; }
        public string Therapy { get; set; }
    }
}