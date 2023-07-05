using System.Windows.Media;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.NurseGUI.PatientHealthcare.Pharmacy
{
    public class PrescriptionViewModel
    {
        public PrescriptionViewModel(Prescription prescription, int Quantity, string medicationName, string doctor)
        {
            Prescription = prescription;
            IsLow = Quantity == 0;
            MedicationName = medicationName;
            DoctorName = doctor;
            Date = prescription.Start.ToString().Split(" ")[0];
        }

        public Prescription Prescription { get; set; }
        public string MedicationName { get; set; }
        public bool IsLow { get; set; }
        public string MealTime => ViewUtil.Translate(Prescription.Instruction);
        public string DoctorName { get; set; }
        public string Date { get; set; }
        public string FirstUse => ViewUtil.Translate(Prescription.FirstUse);
        public string Empty => ViewUtil.Translate(!IsLow);
        public Brush Color => IsLow ? Brushes.Red : Brushes.Black;
    }
}