using System.Windows;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.Core.Users.Model;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicationTherapy
{
    public partial class TherapyInformation : Window
    {
        public TherapyInformation(Patient patient, int medicationID, Therapy therapy)
        {
            InitializeComponent();
            DataContext = new TherapyInformationViewModel(patient, medicationID, therapy, this);
        }
    }
}