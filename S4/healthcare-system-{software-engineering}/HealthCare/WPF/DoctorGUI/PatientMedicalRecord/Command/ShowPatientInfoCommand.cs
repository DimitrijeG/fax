using HealthCare.Application;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.Examination;

namespace HealthCare.WPF.DoctorGUI.PatientMedicalRecord.Command;

public class ShowPatientInfoCommand : CommandBase
{
    private readonly bool _isEdit;
    private readonly PatientService _patientService;
    private readonly ViewModelBase _viewModel;

    public ShowPatientInfoCommand(ViewModelBase view, bool isEdit)
    {
        _patientService = Injector.GetService<PatientService>();
        _viewModel = view;
        _isEdit = isEdit;
    }

    public override void Execute(object parameter)
    {
        var patient = ExtractPatient();
        if (patient is null) return;

        new PatientInformationView(patient, _isEdit).ShowDialog();
        UpdateViewModel();
    }

    private Patient? ExtractPatient()
    {
        if (_viewModel is DoctorMainViewModel doctorMainViewModel)
        {
            var appointment = doctorMainViewModel.SelectedAppointment;
            if (appointment is null)
            {
                ViewUtil.ShowWarning("Morate odabrati pregled/operaciju iz tabele!");
                return null;
            }

            return _patientService.TryGet(appointment.JMBG);
        }

        if (_viewModel is PatientSearchViewModel patientSearchViewModel)
        {
            var selectedPatient = patientSearchViewModel.SelectedPatient;
            if (selectedPatient is null)
            {
                ViewUtil.ShowWarning("Morate odabrati pacijenta iz tabele!");
                return null;
            }

            return _patientService.TryGet(selectedPatient.JMBG);
        }

        if (_viewModel is DoctorExamViewModel doctorExamViewModel)
        {
            var selectedPatient = doctorExamViewModel.SelectedPatient;
            return selectedPatient;
        }

        return null;
    }

    private void UpdateViewModel()
    {
        if (_viewModel is DoctorExamViewModel doctorExamViewModel) doctorExamViewModel.RefreshView();
    }
}