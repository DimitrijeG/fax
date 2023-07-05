using System.Windows;
using System.Windows.Controls;
using HealthCare.Core.Users.Model;
using HealthCare.WPF.Common;
using HealthCare.WPF.NurseGUI.PatientHealthcare.Pharmacy;
using HealthCare.WPF.NurseGUI.PatientHealthcare.Referrals.Specialist;
using HealthCare.WPF.NurseGUI.PatientHealthcare.Referrals.Treatment;

namespace HealthCare.WPF.NurseGUI.Patients
{
    public partial class AllPatientsView
    {
        private PatientListingViewModel _model;
        private Patient? _patient;

        public AllPatientsView()
        {
            InitializeComponent();

            _model = new PatientListingViewModel();
            DataContext = _model;
            _model.Update();

            _patient = null;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void lvPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _patient = (Patient)lvPatients.SelectedItem;
        }

        private void btnShowPatientsReferral_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
                return;

            new SpecialistReferralsView(_patient).ShowDialog();
        }

        private void btnShowTreatmantReferral_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
                return;

            new TreatmantReferralsView(_patient).ShowDialog();
        }

        private void btnPrescribe_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
                return;

            new PrescriptionsView(_patient).ShowDialog();
        }

        private bool Validate()
        {
            if (_patient is null)
            {
                ViewUtil.ShowWarning("Izaberite pacijenta.");
                return false;
            }

            return true;
        }
    }
}