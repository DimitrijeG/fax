using System.Windows;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.Core.Users.Model;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicalPrescription
{
    public partial class PrescriptionView : Window
    {
        public PrescriptionView(Patient patient)
        {
            InitializeComponent();
            DataContext = new PrescriptionViewModel(patient);
        }

        public PrescriptionView(Therapy therapy)
        {
            InitializeComponent();
            DataContext = new PrescriptionViewModel(therapy);
        }
    }
}