using System.Windows;

namespace HealthCare.WPF.DoctorGUI.AbsenceRequesting
{
    public partial class AbsenceRequestView : Window
    {
        public AbsenceRequestView()
        {
            InitializeComponent();
            DataContext = new AbsenceRequestViewModel();
        }
    }
}