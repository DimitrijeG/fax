using System.Windows;
using HealthCare.Core.Users.Model;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Referral
{
    public partial class TreatmentReferralView : Window
    {
        public TreatmentReferralView(Patient patient)
        {
            InitializeComponent();
            DataContext = new TreatmentReferralViewModel(patient);
        }
    }
}