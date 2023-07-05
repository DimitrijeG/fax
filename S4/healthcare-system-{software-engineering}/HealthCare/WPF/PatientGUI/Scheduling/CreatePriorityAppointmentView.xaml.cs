using System.Windows;
using System.Windows.Controls;

namespace HealthCare.WPF.PatientGUI.Scheduling
{
    public partial class CreatePriorityAppointmentView
    {
        public CreatePriorityAppointmentView()
        {
            InitializeComponent();
        }


        private void TbHoursStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = tbHoursStart.Text;
            if (int.TryParse(text, out int hours))
            {
                if (hours > 23 || hours < 0)
                {
                    tbHoursStart.Text = "0";
                }
            }
            else
            {
                tbHoursStart.Text = "0";
            }
        }

        private void TbHoursEnd_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = tbHoursEnd.Text;
            if (int.TryParse(text, out int hours))
            {
                if (hours > 23 || hours < 0)
                {
                    tbHoursEnd.Text = "0";
                }
            }
            else
            {
                tbHoursEnd.Text = "0";
            }
        }

        private void TbMinutesStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = tbMinutesStart.Text;
            if (int.TryParse(text, out int minutes))
            {
                if (minutes > 59 || minutes < 0)
                {
                    tbMinutesStart.Text = "0";
                }
            }
            else
            {
                tbMinutesStart.Text = "0";
            }
        }

        private void TbMinutesEnd_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = tbMinutesEnd.Text;
            if (int.TryParse(text, out int minutes))
            {
                if (minutes > 59 || minutes < 0)
                {
                    tbMinutesEnd.Text = "0";
                }
            }
            else
            {
                tbMinutesEnd.Text = "0";
            }
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}