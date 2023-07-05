using System.Windows;

namespace HealthCare.WPF.DoctorGUI.PatientMedicalRecord
{
    public partial class PatientSearchView : Window
    {
        public PatientSearchView()
        {
            InitializeComponent();
            DataContext = new PatientSearchViewModel();
        }
    }
}