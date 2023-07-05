using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using HealthCare.Core.Users.Model;
using HealthCare.WPF.Common;
using HealthCare.WPF.DoctorGUI.PatientMedicalRecord.Command;

namespace HealthCare.WPF.DoctorGUI.PatientMedicalRecord;

public class PatientInformationViewModel : ViewModelBase
{
    private readonly ObservableCollection<string> _allergies;
    private readonly ObservableCollection<string> _previousDiseases;
    private readonly Patient _selectedPatient;
    private string _allergy;
    private string _disease;
    private float _height;
    private string _selectedAllergy;
    private string _selectedDisease;
    private float _weight;

    public PatientInformationViewModel(Patient patient, bool isEditing)
    {
        _selectedPatient = patient;
        IsFocusable = isEditing;
        IsReadOnly = !isEditing;

        SaveChangesCommand = new SavePatientChangesCommand(patient, this);
        NewDiseaseCommand = new AddDiseaseCommand(this);
        RemoveDiseaseCommand = new RemoveDiseaseCommand(this);
        NewAllergyCommand = new AddAllergyCommand(this);
        RemoveAllergyCommand = new RemoveAllergyCommand(this);

        _allergies = new ObservableCollection<string>();
        _previousDiseases = new ObservableCollection<string>();
        LoadDataIntoView(patient, isEditing);
    }

    public IEnumerable<string> PreviousDisease => _previousDiseases;
    public IEnumerable<string> Allergies => _allergies;
    public ICommand SaveChangesCommand { get; }
    public ICommand NewDiseaseCommand { get; }
    public ICommand RemoveDiseaseCommand { get; }
    public ICommand NewAllergyCommand { get; }
    public ICommand RemoveAllergyCommand { get; }
    public Visibility GridVisibility { get; private set; }

    public bool IsFocusable { get; }

    public bool IsReadOnly { get; }

    public string Name { get; set; }

    public string LastName { get; set; }

    public string JMBG { get; set; }

    public DateTime Birthday { get; set; }

    public string Gender { get; set; }

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

    public string SelectedAllergy
    {
        get => _selectedAllergy;
        set
        {
            _selectedAllergy = value;
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

    public string Allergy
    {
        get => _allergy;
        set
        {
            _allergy = value;
            OnPropertyChanged();
        }
    }

    public void LoadDataIntoView(Patient patient, bool isEditing)
    {
        Name = patient.Name;
        LastName = patient.LastName;
        Birthday = patient.BirthDate;
        Gender = ViewUtil.Translate(patient.Gender);
        JMBG = patient.JMBG;

        if (patient.MedicalRecord != null)
        {
            Weight = patient.MedicalRecord.Weight;
            Height = patient.MedicalRecord.Height;
            Update();
        }

        GridVisibility = isEditing ? Visibility.Visible : Visibility.Collapsed;
    }

    private void Update()
    {
        UpdateAllergies();
        UpdateDisease();
    }

    private void UpdateDisease()
    {
        _previousDiseases.Clear();
        if (_selectedPatient.MedicalRecord == null) return;
        foreach (var disease in _selectedPatient.MedicalRecord.MedicalHistory)
            _previousDiseases.Add(disease);
    }

    private void UpdateAllergies()
    {
        _allergies.Clear();
        if (_selectedPatient.MedicalRecord == null) return;
        foreach (var allergy in _selectedPatient.MedicalRecord.Allergies)
            _allergies.Add(allergy);
    }

    public void AddPreviousDisease(string disease)
    {
        _previousDiseases.Add(disease);
    }

    public void RemovePreviousDisease(string disease)
    {
        _previousDiseases.Remove(disease);
    }

    public void AddAllergy(string allergy)
    {
        _allergies.Add(allergy);
    }

    public void RemoveAllergy(string allergy)
    {
        _allergies.Remove(allergy);
    }
}