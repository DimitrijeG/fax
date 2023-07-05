using System.ComponentModel;
using System.Windows;
using HealthCare.WPF.NurseGUI.PatientHealthcare.Pharmacy;
using HealthCare.WPF.NurseGUI.PatientHealthcare.Treatments.Visits;
using HealthCare.WPF.NurseGUI.Patients;
using HealthCare.WPF.NurseGUI.Reception;
using HealthCare.WPF.NurseGUI.Scheduling;
using HealthCare.WPF.PatientGUI.Communication.Chats;

namespace HealthCare.WPF.NurseGUI
{
    public partial class NurseMenu : Window
    {
        private MainWindow _window;

        public NurseMenu(MainWindow window)
        {
            InitializeComponent();
            _window = window;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Close();
            _window.Show();
        }

        private void mnuCRUD_Click(object sender, RoutedEventArgs e)
        {
            new NurseMainView().ShowDialog();
        }

        private void mnuReception_Click(object sender, RoutedEventArgs e)
        {
            new MainReceptionView().ShowDialog();
        }

        private void mnuUrgent_Click(object sender, RoutedEventArgs e)
        {
            new CreateUrgentAppointmentView().ShowDialog();
        }

        private void mnuReferral_Click(object sender, RoutedEventArgs e)
        {
            new AllPatientsView().ShowDialog();
        }

        private void mnuOrder_Click(object sender, RoutedEventArgs e)
        {
            new MedicationOrderView().ShowDialog();
        }

        private void mnuVisit_Click(object sender, RoutedEventArgs e)
        {
            new VisitListingView().ShowDialog();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _window.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ChatView().Show();
        }
    }
}