using System.Windows;

namespace HealthCare.WPF.ManagerGUI.HumanResources
{
    public partial class AbsenceRequestListingView : Window
    {
        public AbsenceRequestListingView()
        {
            InitializeComponent();

            DataContext = new AbsenceRequestListingViewModel(this);
        }
    }
}