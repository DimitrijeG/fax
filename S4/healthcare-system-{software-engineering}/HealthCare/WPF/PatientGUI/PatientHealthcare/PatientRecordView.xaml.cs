using System.Windows.Controls;
using HealthCare.WPF.NurseGUI.Scheduling;

namespace HealthCare.WPF.PatientGUI.PatientHealthcare
{
    public partial class PatientRecordView
    {
        PatientRecordViewModel model;

        public PatientRecordView()
        {
            model = new PatientRecordViewModel();
            DataContext = model;
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model.Sort(cbSort.SelectedValue.ToString());
        }

        private void TbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            model.Filter(tbFilter.Text);
        }

        private void ListViewRecord_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var appointment = (AppointmentViewModel)listViewRecord.SelectedItem;

            if (listViewRecord.SelectedItems.Count == 1)
            {
                model.ShowAnamnesis(appointment.Appointment);
            }
        }
    }
}