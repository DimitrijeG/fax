using HealthCare.Application;
using HealthCare.Core.PatientHealthcare.HealthcareTreatment;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Treatments
{
    public class TreatmentDTO : ViewModelBase
    {
        private readonly Treatment _treatment;

        public TreatmentDTO(Treatment treatment)
        {
            _treatment = treatment;
            TreatmentReferral referral = Injector.GetService<TreatmentReferralService>().Get(treatment.ReferralId);
            Patient patient = Injector.GetService<PatientService>().Get(referral.PatientJMBG);
            PatientNameAndLastName = patient.Name + " " + patient.LastName;
            PatientJMBG = patient.JMBG;
        }

        public int TreatmentId => _treatment.Id;
        public string PatientNameAndLastName { get; }
        public string PatientJMBG { get; }
        public int RoomId => _treatment.RoomId;
        public string Start => ViewUtil.ToString(_treatment.TreatmentDuration.Start, true);
        public string End => ViewUtil.ToString(_treatment.TreatmentDuration.End, true);
    }
}