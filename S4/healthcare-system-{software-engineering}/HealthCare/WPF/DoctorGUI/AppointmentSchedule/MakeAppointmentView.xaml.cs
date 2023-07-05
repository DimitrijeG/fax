using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using HealthCare.Core.Scheduling.Examination;

namespace HealthCare.WPF.DoctorGUI.AppointmentSchedule
{
    public partial class MakeAppointmentView : Window
    {
        public MakeAppointmentView(DoctorMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = new MakeAppointmentViewModel(viewModel, this);
        }

        public MakeAppointmentView(DoctorMainViewModel viewModel, Appointment appointment)
        {
            InitializeComponent();
            DataContext = new MakeAppointmentViewModel(appointment, viewModel, this);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}