using System.Windows;
using System.Windows.Input;
using HealthCare.Application;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.Core.Users.Model;
using HealthCare.WPF.Common;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicationTherapy.Command;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicationTherapy;

public class TherapyInformationViewModel : ViewModelBase
{
    private readonly Medication _medication;
    private readonly MedicationService _medicationService;

    public TherapyInformationViewModel(Patient patient, int medicationID, Therapy therapy, Window window)
    {
        _medicationService = Injector.GetService<MedicationService>();
        ExaminedPatient = patient;
        _medication = _medicationService.Get(medicationID);
        Therapy = therapy;
        AddMedicationToTherapyCommand = new AddTherapyToReferralCommand(window, this);
        MedicationName = _medication.Name;
        NoPreference = true;
    }

    public int MedicationID => _medication.Id;
    public Therapy Therapy { get; set; }
    public Patient ExaminedPatient { get; set; }

    public string MedicationName { get; set; }
    public int DailyDosage { get; set; }
    public int HoursBetweenConsumption { get; set; }
    public int ConsumptionDays { get; set; }
    public bool BeforeMeal { get; set; }
    public bool DuringMeal { get; set; }
    public bool AfterMeal { get; set; }
    public bool NoPreference { get; set; }

    public ICommand AddMedicationToTherapyCommand { get; }
}