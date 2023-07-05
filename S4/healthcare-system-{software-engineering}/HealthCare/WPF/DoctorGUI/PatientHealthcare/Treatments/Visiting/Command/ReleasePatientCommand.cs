using System.Windows;
using HealthCare.Application;
using HealthCare.Core.PatientHealthcare.HealthcareTreatment;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Treatments.Visiting.Command
{
    public class ReleasePatientCommand : CommandBase
    {
        private readonly Treatment _treatment;
        private readonly TreatmentService _treatmentService;
        private readonly Window _window;

        public ReleasePatientCommand(Window window, Treatment treatment)
        {
            _treatmentService = Injector.GetService<TreatmentService>();

            _window = window;
            _treatment = treatment;
        }

        public override void Execute(object parameter)
        {
            MessageBoxResult answer = MessageBox.Show("Da li je potrebna kontrola?", "Kontrola nakon otpusta",
                MessageBoxButton.YesNo,
                MessageBoxImage.Information);
            if (answer == MessageBoxResult.Yes)
            {
                new PatientReleaseAppointmentView(_treatment).ShowDialog();
            }

            _treatmentService.Remove(_treatment.Id);
            _window.Close();
        }
    }
}