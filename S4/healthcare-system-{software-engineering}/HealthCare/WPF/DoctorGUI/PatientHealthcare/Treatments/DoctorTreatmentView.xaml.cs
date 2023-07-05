using System.Windows;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Treatments
{
    public partial class DoctorTreatmentView : Window
    {
        public DoctorTreatmentView()
        {
            InitializeComponent();
            DataContext = new DoctorTreatmentViewModel();
        }
    }
}