using System.Windows.Input;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Pharmacy;

public class MedicationDTO : ViewModelBase
{
    private readonly Medication _medication;
    private bool _initialTherapy;

    public MedicationDTO(Medication medication)
    {
        _medication = medication;
        _initialTherapy = false;

        ShowMedicationInformationCommand = new ShowMedicationCommand(medication);
    }

    public int MedicationId => _medication.Id;
    public string MedicationName => _medication.Name;
    public string Description => _medication.Description;
    public string Ingredients => ViewUtil.ToString(_medication.Ingredients);

    public bool InitialTherapy
    {
        get => _initialTherapy;
        set
        {
            _initialTherapy = value;
            OnPropertyChanged();
        }
    }

    public ICommand ShowMedicationInformationCommand { get; }
}