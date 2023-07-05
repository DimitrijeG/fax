using System.Windows;
using HealthCare.Core.Users.Model;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Referral
{
    public partial class SpecialistReferralView : Window
    {
        public SpecialistReferralView(Patient patient)
        {
            InitializeComponent();
            DataContext = new SpecialistReferralViewModel(patient);
        }
    }
}