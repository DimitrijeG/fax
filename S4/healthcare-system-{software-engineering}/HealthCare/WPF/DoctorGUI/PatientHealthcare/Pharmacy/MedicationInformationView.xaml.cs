using System.Windows;
using HealthCare.Core.PatientHealthcare.Pharmacy;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Pharmacy
{
    public partial class MedicationInformationView : Window
    {
        public MedicationInformationView(Medication medication)
        {
            InitializeComponent();
            DataContext = new MedicationInformationViewModel(medication);
        }
    }
}