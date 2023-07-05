using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.WPF.Common.Command;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicalPrescription;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.Treatments.Visiting;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicationTherapy.Command
{
    public class ShowAddTherapyDialogCommand : CommandBase
    {
        private readonly Therapy _therapy;
        private readonly DoctorTreatmentVisitViewModel _viewModel;

        public ShowAddTherapyDialogCommand(DoctorTreatmentVisitViewModel viewModel, Therapy therapy)
        {
            _therapy = therapy;
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            new PrescriptionView(_therapy).ShowDialog();
            _viewModel.Update();
        }
    }
}