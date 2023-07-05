using System.ComponentModel;
using System.Windows;
using HealthCare.WPF.PatientGUI.Communication.Chats;

namespace HealthCare.WPF.DoctorGUI
{
    public partial class DoctorMainView : Window
    {
        private MainWindow _loginWindow;

        public DoctorMainView(MainWindow loginWindow)
        {
            _loginWindow = loginWindow;
            InitializeComponent();
            DataContext = new DoctorMainViewModel(this);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _loginWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ChatView().Show();
        }
    }
}