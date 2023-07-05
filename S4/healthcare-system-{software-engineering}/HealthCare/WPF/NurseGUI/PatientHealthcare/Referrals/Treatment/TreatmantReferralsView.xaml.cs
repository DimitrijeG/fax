using System.Windows;
using HealthCare.Core.Users.Model;

namespace HealthCare.WPF.NurseGUI.PatientHealthcare.Referrals.Treatment
{
    public partial class TreatmantReferralsView : Window
    {
        public TreatmantReferralsView(Patient patient)
        {
            InitializeComponent();
            DataContext = new TreatmantReferralListingViewModel(patient, this);
        }
    }
}