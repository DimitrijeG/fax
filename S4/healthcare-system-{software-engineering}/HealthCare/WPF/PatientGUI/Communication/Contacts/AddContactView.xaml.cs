using System.Windows;
using System.Windows.Input;
using HealthCare.WPF.PatientGUI.Communication.Chats;

namespace HealthCare.WPF.PatientGUI.Communication.Contacts
{
    public partial class AddContactView
    {
        public AddContactView(ChatViewModel previousModel)
        {
            InitializeComponent();
            DataContext = new AddContactViewModel(previousModel);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}