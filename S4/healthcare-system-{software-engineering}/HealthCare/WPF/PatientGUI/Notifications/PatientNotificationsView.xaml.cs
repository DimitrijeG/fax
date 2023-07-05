using System.Windows;
using System.Windows.Controls;
using HealthCare.Application;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.PatientGUI.Notifications
{
    /// <summary>
    /// Interaction logic for PatientNotificationsView.xaml
    /// </summary>
    public partial class PatientNotificationsView : UserControl
    {
        private readonly PatientService _patientService;
        PatientNotificationsViewModel _viewModel;

        public PatientNotificationsView()
        {
            _patientService = Injector.GetService<PatientService>();
            _viewModel = new PatientNotificationsViewModel();
            _viewModel.LoadNotifications();
            DataContext = _viewModel;
            InitializeComponent();
        }


        private void expandButton_Click(object sender, RoutedEventArgs e)
        {
            new CreateNotificationView(_viewModel).Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string hoursText = hoursTextBox.Text;
            if (int.TryParse(hoursText, out int notificationHours))
            {
                if (notificationHours <= 0)
                {
                    ViewUtil.ShowWarning("Broj sati mora biti pozitivan broj!");
                    return;
                }
                else
                {
                    Patient patient = (Patient)Context.Current;
                    patient.NotificationHours = notificationHours;
                    _patientService.Update(patient);
                    _viewModel.LoadNotifications();
                }
            }
            else
            {
                ViewUtil.ShowWarning("Broj sati mora biti broj");
            }
        }
    }
}