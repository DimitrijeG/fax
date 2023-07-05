using System.ComponentModel;
using System.Windows;
using HealthCare.Application;
using HealthCare.Application.Exceptions;
using HealthCare.Core.Notifications;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;
using HealthCare.WPF.DoctorGUI;
using HealthCare.WPF.ManagerGUI;
using HealthCare.WPF.NurseGUI;
using HealthCare.WPF.PatientGUI;

namespace HealthCare
{
    public partial class MainWindow : Window
    {
        private readonly LoginService _loginService;
        private readonly NotificationService _notificationService;

        public MainWindow()
        {
            InitializeComponent();

            _notificationService = Injector.GetService<NotificationService>();
            _loginService = Injector.GetService<LoginService>();
        }

        private void btnQuitApp_Click(object sender, RoutedEventArgs e)
        {
            ExitApp();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUserName.Text;
            string password = txtPassword.Password;

            try
            {
                switch (_loginService.Login(username, password))
                {
                    case Role.Manager:
                        new ManagerMenu(this).Show();
                        break;
                    case Role.Doctor:
                        ShowNotifications();
                        new DoctorMainView(this).Show();
                        break;
                    case Role.Nurse:
                        new NurseMenu(this).Show();

                        break;
                    case Role.Patient:
                        ShowNotifications();
                        new PatientMainView(this).Show();
                        break;
                }

                txtUserName.Clear();
                txtPassword.Clear();
                Hide();
            }
            catch (LoginException ex)
            {
                ViewUtil.ShowWarning(ex.Message);
            }
        }

        private void ShowNotifications()
        {
            foreach (var notification in _notificationService.GetForUser(Context.Current.JMBG))
            {
                ViewUtil.ShowInformation(notification.Display());
                _notificationService.Update(notification);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            ExitApp();
        }

        public void ExitApp()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}