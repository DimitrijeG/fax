using HealthCare.Application;
using HealthCare.Core.Interior;
using HealthCare.Core.PatientHealthcare.HealthcareTreatment;
using HealthCare.Core.Users.Service;

namespace HealthCare.WPF.NurseGUI.PatientHealthcare.Treatments
{
    public class TreatmentViewModel
    {
        public TreatmentViewModel(Treatment treatment)
        {
            var patientService = Injector.GetService<PatientService>();
            var treatmentReferralService = Injector.GetService<TreatmentReferralService>();
            var roomService = Injector.GetService<RoomService>();

            var jmbg = treatmentReferralService.Get(treatment.ReferralId).PatientJMBG;
            var patient = patientService.Get(jmbg);

            Id = treatment.Id;
            Name = patient.Name;
            LastName = patient.LastName;
            RoomName = roomService.Get(treatment.RoomId).Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string RoomName { get; set; }
    }
}