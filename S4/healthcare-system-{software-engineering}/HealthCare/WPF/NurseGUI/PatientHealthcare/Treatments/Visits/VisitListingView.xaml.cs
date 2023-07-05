using System.Windows;

namespace HealthCare.WPF.NurseGUI.PatientHealthcare.Treatments.Visits
{
    public partial class VisitListingView : Window
    {
        public VisitListingView()
        {
            InitializeComponent();
            DataContext = new VisitListingViewModel(this);
        }
    }
}