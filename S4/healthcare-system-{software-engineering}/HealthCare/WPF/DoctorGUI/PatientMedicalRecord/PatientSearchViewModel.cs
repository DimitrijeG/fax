using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HealthCare.Application;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;
using HealthCare.WPF.DoctorGUI.PatientMedicalRecord.Command;

namespace HealthCare.WPF.DoctorGUI.PatientMedicalRecord;

public class PatientSearchViewModel : ViewModelBase
{
    private readonly AppointmentService _appointmentService;

    private readonly ObservableCollection<PatientDTO> _patients;
    private readonly PatientService _patientService;

    private PatientDTO _selectedPatient;

    public PatientSearchViewModel()
    {
        _appointmentService = Injector.GetService<AppointmentService>();
        _patientService = Injector.GetService<PatientService>();
        _patients = new ObservableCollection<PatientDTO>();
        ShowEditPatientCommand = new ShowPatientInfoCommand(this, true);
        Update();
    }

    public IEnumerable<PatientDTO> Patients => _patients;

    public PatientDTO SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            _selectedPatient = value;
            OnPropertyChanged();
        }
    }

    public ICommand ShowEditPatientCommand { get; }

    public void Update()
    {
        _patients.Clear();
        foreach (var patientJMBG in _appointmentService.GetExaminedPatients(Context.Current.JMBG))
        {
            var patient = _patientService.Get(patientJMBG);
            _patients.Add(new PatientDTO(patient));
        }
    }
}