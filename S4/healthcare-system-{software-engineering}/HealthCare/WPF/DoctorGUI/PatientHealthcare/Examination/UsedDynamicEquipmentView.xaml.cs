using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Examination
{
    public partial class UsedDynamicEquipmentView : Window
    {
        public UsedDynamicEquipmentView(int roomId)
        {
            InitializeComponent();
            DataContext = new UsedDynamicEquipmentViewModel(this, roomId);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}