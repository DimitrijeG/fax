using System.Windows;
using HealthCare.Core.Scheduling.Examination;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Examination
{
    public partial class DoctorExamView : Window
    {
        public DoctorExamView(Appointment appointment)
        {
            InitializeComponent();
            DataContext = new DoctorExamViewModel(this, appointment);
        }
    }
}