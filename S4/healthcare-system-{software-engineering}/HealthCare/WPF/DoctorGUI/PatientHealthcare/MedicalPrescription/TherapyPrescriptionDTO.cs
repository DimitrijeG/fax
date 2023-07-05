using HealthCare.Application;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicalPrescription
{
    public class TherapyPrescriptionDTO : ViewModelBase
    {
        private readonly Medication _medication;
        private readonly Prescription _prescription;

        public TherapyPrescriptionDTO(Prescription prescription)
        {
            _prescription = prescription;
            _medication = Injector.GetService<MedicationService>().Get(_prescription.MedicationId);
        }

        public int PrescriptionID => _prescription.Id;
        public int MedicationID => _medication.Id;
        public string MedicationName => _medication.Name;
        public int DailyDosage => _prescription.DailyDosage;
        public int ConsumptionDays => _prescription.ConsumptionDays;
        public string Instruction => ViewUtil.Translate(_prescription.Instruction);
    }
}