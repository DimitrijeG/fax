using System.Windows;
using HealthCare.Core.PatientHealthcare.HealthcareTreatment;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Treatments.Visiting
{
    public partial class DoctorTreatmentVisitView : Window
    {
        public DoctorTreatmentVisitView(Treatment treatment)
        {
            InitializeComponent();
            DataContext = new DoctorTreatmentVisitViewModel(this, treatment);
        }
    }
}