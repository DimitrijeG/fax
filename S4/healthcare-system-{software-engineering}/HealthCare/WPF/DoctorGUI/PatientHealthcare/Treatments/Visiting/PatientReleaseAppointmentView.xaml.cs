using System.Windows;
using HealthCare.Core.PatientHealthcare.HealthcareTreatment;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicationTherapy.Command;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Treatments.Visiting
{
    public partial class PatientReleaseAppointmentView : Window
    {
        public PatientReleaseAppointmentView(Treatment treatment)
        {
            InitializeComponent();
            DataContext = new PatientReleaseAppointmentViewModel(this, treatment);
        }
    }
}