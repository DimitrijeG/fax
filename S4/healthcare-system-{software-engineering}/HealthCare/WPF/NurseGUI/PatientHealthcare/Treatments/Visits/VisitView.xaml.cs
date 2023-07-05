using System.Windows;
using HealthCare.Core.PatientHealthcare.HealthcareTreatment;

namespace HealthCare.WPF.NurseGUI.PatientHealthcare.Treatments.Visits
{
    public partial class VisitView : Window
    {
        public VisitView(Visit visit)
        {
            InitializeComponent();
            DataContext = new VisitViewModel(visit, this);
        }
    }
}