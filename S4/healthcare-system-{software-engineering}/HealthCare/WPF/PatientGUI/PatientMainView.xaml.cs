using System.Windows;
using System.Windows.Controls;
using HealthCare.Application;
using HealthCare.WPF.PatientGUI.Notifications;
using HealthCare.WPF.PatientGUI.PatientHealthcare;
using HealthCare.WPF.PatientGUI.PatientSatisfaction;
using HealthCare.WPF.PatientGUI.Scheduling;
using HealthCare.WPF.PatientGUI.Scheduling.DoctorListing;

namespace HealthCare.WPF.PatientGUI
{
    public partial class PatientMainView
    {
        MainWindow _mainWindow;

        public PatientMainView(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            this.labelUsername.Text = Context.Current.Name;
            btnCrud.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            btnCrud.Focus();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainContentGrid.Content = new AppointmentCrudView();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mainContentGrid.Content = new CreatePriorityAppointmentView();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            mainContentGrid.Content = new PatientRecordView();
        }

        private void btnDoctors_Click(object sender, RoutedEventArgs e)
        {
            mainContentGrid.Content = new DoctorListingView(this);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Close();
            _mainWindow.Show();
        }

        private void btnNotifications_Click(object sender, RoutedEventArgs e)
        {
            mainContentGrid.Content = new PatientNotificationsView();
        }

        private void SetWindow(UserControl control)
        {
            mainContentGrid.Content = control;
        }

        private void btnHospitalSurvey_Click(object sender, RoutedEventArgs e)
        {
            mainContentGrid.Content = new SurveyHospitalView();
        }

        private void btnDoctorSurvey_Click(object sender, RoutedEventArgs e)
        {
            mainContentGrid.Content = new SurveyDoctorView();
        }
    }
}