using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using HealthCare.Application;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.Examination.Command;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicalPrescription;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.Referral.Command;
using HealthCare.WPF.DoctorGUI.PatientMedicalRecord.Command;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Examination;

public class DoctorExamViewModel : ViewModelBase
{
    private readonly AnamnesisService _anamnesisService;
    private readonly Appointment _appointment;
    private readonly PatientService _patientService;
    private ObservableCollection<string> _allergies;
    private string _conclusion;
    private string _disease;
    private float _height;
    private string _jmbg;
    private string _lastName;
    private string _name;

    private ObservableCollection<string> _previousDiseases;
    private string _selectedDisease;
    private Patient _selectedPatient;
    private string _symptoms;
    private float _weight;


    public DoctorExamViewModel(Window window, Appointment appointment)
    {
        _anamnesisService = Injector.GetService<AnamnesisService>();
        _patientService = Injector.GetService<PatientService>();
        _appointment = appointment;
        _selectedPatient = _patientService.Get(appointment.PatientJMBG);


        UpdatePatientCommand = new ShowPatientInfoCommand(this, true);
        FinishExaminationCommand = new FinishExaminationCommand(window, appointment, this);
        MakeSpecialistReferralCommand = new ShowSpecialistReferralViewCommand(_selectedPatient);
        MakeTreatmentReferralCommand = new ShowTreatmentReferralViewCommand(_selectedPatient);
        MakePrescriptionCommand = new ShowPrescriptionViewCommand(_selectedPatient);
        LoadView();
    }

    public IEnumerable<string> Allergies => _allergies;
    public IEnumerable<string> PreviousDisease => _previousDiseases;

    public ICommand FinishExaminationCommand { get; }
    public ICommand MakeSpecialistReferralCommand { get; }
    public ICommand MakeTreatmentReferralCommand { get; }
    public ICommand MakePrescriptionCommand { get; }
    public ICommand UpdatePatientCommand { get; }

    public Patient SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            _selectedPatient = value;
            OnPropertyChanged();
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            OnPropertyChanged();
        }
    }

    public string JMBG
    {
        get => _jmbg;
        set
        {
            _jmbg = value;
            OnPropertyChanged();
        }
    }

    public float Height
    {
        get => _height;
        set
        {
            _height = value;
            OnPropertyChanged();
        }
    }

    public float Weight
    {
        get => _weight;
        set
        {
            _weight = value;
            OnPropertyChanged();
        }
    }

    public string SelectedDisease
    {
        get => _selectedDisease;
        set
        {
            _selectedDisease = value;
            OnPropertyChanged();
        }
    }

    public string Disease
    {
        get => _disease;
        set
        {
            _disease = value;
            OnPropertyChanged();
        }
    }

    public string Symptoms
    {
        get => _symptoms;
        set
        {
            _disease = value;
            OnPropertyChanged();
        }
    }

    public string Conclusion
    {
        get => _conclusion;
        set
        {
            _conclusion = value;
            OnPropertyChanged();
        }
    }

    private void LoadView()
    {
        _name = _selectedPatient.Name;
        _lastName = _selectedPatient.LastName;
        _jmbg = _selectedPatient.JMBG;
        _height = _selectedPatient.MedicalRecord.Height;
        _weight = _selectedPatient.MedicalRecord.Weight;

        var anamnesis = _anamnesisService.Get(_appointment.AnamnesisID);
        _symptoms = string.Join(", ", anamnesis.Symptoms);
        _previousDiseases = new ObservableCollection<string>();
        _allergies = new ObservableCollection<string>();
        Update();
    }

    private void Update()
    {
        UpdateDiseases();
        UpdateAllergies();
    }

    private void UpdateDiseases()
    {
        _previousDiseases.Clear();
        foreach (var disease in _selectedPatient.MedicalRecord.MedicalHistory) _previousDiseases.Add(disease);
    }

    private void UpdateAllergies()
    {
        _allergies.Clear();
        foreach (var allergy in _selectedPatient.MedicalRecord.Allergies) _allergies.Add(allergy);
    }

    public void RefreshView()
    {
        Update();
        Height = SelectedPatient.MedicalRecord.Height;
        Weight = SelectedPatient.MedicalRecord.Weight;
    }
}