using System.Windows.Controls;
using HealthCare.Core.Users.Model;

namespace HealthCare.WPF.PatientGUI.Scheduling.DoctorListing
{
    public partial class DoctorListingView
    {
        PatientMainView _mainView;
        DoctorListingViewModel model;

        public DoctorListingView(PatientMainView mainView)
        {
            model = new DoctorListingViewModel();
            DataContext = model;
            _mainView = mainView;
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            model.Filter(tbFilter.Text);
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model.Sort(cbSort.SelectedValue.ToString());
        }

        private void listViewRecord_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewRecord.SelectedItems.Count == 1)
            {
                Doctor doctor = (Doctor)listViewRecord.SelectedItem;
                AppointmentCrudView appointmentCrudView = new AppointmentCrudView(doctor);
                _mainView.mainContentGrid.Content = appointmentCrudView;
            }
        }
    }
}