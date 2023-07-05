using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HealthCare.Application;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.Core.Users.Model;
using HealthCare.WPF.Common;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicationTherapy.Command;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.Pharmacy;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicalPrescription;

public class PrescriptionViewModel : ViewModelBase
{
    private readonly ObservableCollection<MedicationDTO> _medications;
    private readonly MedicationService _medicationService;

    public PrescriptionViewModel()
    {
        _medicationService = Injector.GetService<MedicationService>();
        BeforeMeal = true;
        _medications = new ObservableCollection<MedicationDTO>();
    }

    public PrescriptionViewModel(Patient patient) : this()
    {
        MakePrescriptionCommand = new AddPrescriptionCommand(patient, this);

        Update();
    }

    public PrescriptionViewModel(Therapy therapy) : this()
    {
        MakePrescriptionCommand = new AddMedicationToTreatmentCommand(this, therapy);

        Update();
    }

    public MedicationDTO SelectedMedication { get; set; }

    public IEnumerable<MedicationDTO> Medications => _medications;

    public bool BeforeMeal { get; set; }
    public bool DuringMeal { get; set; }
    public bool AfterMeal { get; set; }
    public bool NoPreference { get; set; }
    public int DailyDosage { get; set; }
    public int HoursBetweenConsumption { get; set; }
    public int ConsumptionDays { get; set; }

    public ICommand MakePrescriptionCommand { get; }

    private void Update()
    {
        _medications.Clear();
        foreach (var medication in _medicationService.GetAll()) _medications.Add(new MedicationDTO(medication));
    }
}