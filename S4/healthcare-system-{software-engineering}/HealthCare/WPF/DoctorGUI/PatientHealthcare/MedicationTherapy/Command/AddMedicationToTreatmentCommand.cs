using HealthCare.Application;
using HealthCare.Application.Exceptions;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicalPrescription;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicationTherapy.Command
{
    public class AddMedicationToTreatmentCommand : CommandBase
    {
        private readonly MedicationService _medicationService;
        private readonly PatientService _patientService;
        private readonly PrescriptionService _prescriptionService;
        private readonly PrescriptionViewModel _prescriptionViewModel;
        private readonly Therapy _therapy;
        private readonly TherapyService _therapyService;

        public AddMedicationToTreatmentCommand(PrescriptionViewModel prescriptionViewModel, Therapy therapy)
        {
            _prescriptionViewModel = prescriptionViewModel;
            _prescriptionService = Injector.GetService<PrescriptionService>(Injector.THERAPY_PRESCRIPTION_S);
            _medicationService = Injector.GetService<MedicationService>();
            _therapyService = Injector.GetService<TherapyService>();
            _patientService = Injector.GetService<PatientService>();
            _therapy = therapy;
        }

        public override void Execute(object parameter)
        {
            try
            {
                Validate();
                MakePrescription();
                ViewUtil.ShowInformation("Recept uspesno izdat!");
            }
            catch (ValidationException ex)
            {
                ViewUtil.ShowWarning(ex.Message);
            }
        }

        private void MakePrescription()
        {
            var dailyDosage = _prescriptionViewModel.DailyDosage;
            var hoursBetweenConsumption = _prescriptionViewModel.HoursBetweenConsumption;
            var consumptionDays = _prescriptionViewModel.ConsumptionDays;
            var selectedMedication = _prescriptionViewModel.SelectedMedication.MedicationId;
            var doctorJMBG = Context.Current.JMBG;
            var mealTime = GetMealTime();

            var patient = _patientService.Get(_therapy.PatientJMBG);
            CheckPatientAllergies(patient, selectedMedication);

            var prescription = new Prescription(selectedMedication, mealTime, patient.JMBG, doctorJMBG, dailyDosage,
                hoursBetweenConsumption, consumptionDays);

            _prescriptionService.Add(prescription);
            _therapy.InitialMedication.Add(prescription.Id);
            _therapyService.Update(_therapy);
        }

        private MealTime GetMealTime()
        {
            if (_prescriptionViewModel.BeforeMeal) return MealTime.BeforeMeal;

            if (_prescriptionViewModel.AfterMeal) return MealTime.AfterMeal;

            if (_prescriptionViewModel.DuringMeal) return MealTime.DuringMeal;

            return MealTime.NoPreference;
        }

        private void Validate()
        {
            if (_prescriptionViewModel.DailyDosage <= 0)
                throw new ValidationException("Dnevna doza unosa nije validna");

            if (_prescriptionViewModel.HoursBetweenConsumption <= 0)
                throw new ValidationException("Sati izmedju konzumacije nisu validni");

            if (_prescriptionViewModel.ConsumptionDays <= 0)
                throw new ValidationException("Broj dana konzumacije nije validan");

            if (_prescriptionViewModel.HoursBetweenConsumption * _prescriptionViewModel.DailyDosage > 23)
                throw new ValidationException("Ne ispravan unos vremena i leka na dnevnom nivou");

            if (_prescriptionViewModel.SelectedMedication is null)
                throw new ValidationException("Niste odabrali lek");
        }

        private void CheckPatientAllergies(Patient patient, int medicationID)
        {
            var medication = _medicationService.Get(medicationID);

            if (patient.IsAllergic(medication.Ingredients))
                throw new ValidationException("Pacijent je alergican na lek: " + medication.Name);
        }
    }
}