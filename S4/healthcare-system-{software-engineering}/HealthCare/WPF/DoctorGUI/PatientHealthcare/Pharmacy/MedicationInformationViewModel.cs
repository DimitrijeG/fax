using System.Collections.Generic;
using System.Collections.ObjectModel;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Pharmacy;

public class MedicationInformationViewModel : ViewModelBase
{
    private readonly Medication _medication;
    private readonly ObservableCollection<string> _medicationIngredients;
    private string _description;
    private int _id;
    private string _name;

    public MedicationInformationViewModel(Medication medication)
    {
        _medication = medication;
        _medicationIngredients = new ObservableCollection<string>();

        Description = medication.Description;
        ID = medication.Id;
        Name = medication.Name;

        Update();
    }

    public IEnumerable<string> MedicationIngredients => _medicationIngredients;

    public int ID
    {
        get => _id;
        set
        {
            _id = value;
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

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }

    private void Update()
    {
        _medicationIngredients.Clear();
        foreach (var ingredient in _medication.Ingredients) _medicationIngredients.Add(ingredient);
    }
}