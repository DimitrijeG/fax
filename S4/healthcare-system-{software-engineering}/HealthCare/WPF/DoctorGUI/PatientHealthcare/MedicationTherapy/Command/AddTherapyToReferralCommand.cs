using System.Windows;
using HealthCare.Application;
using HealthCare.Application.Exceptions;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.Core.Users.Model;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicationTherapy.Command;

public class AddTherapyToReferralCommand : CommandBase
{
    private readonly Patient _examinedPatient;
    private readonly int _medicationId;
    private readonly PrescriptionService _prescriptionService;
    private readonly TherapyInformationViewModel _therapyInformationViewModel;
    private readonly Window _window;

    public AddTherapyToReferralCommand(Window window, TherapyInformationViewModel therapyInformationViewModel)
    {
        _prescriptionService = Injector.GetService<PrescriptionService>(Injector.THERAPY_PRESCRIPTION_S);
        _window = window;
        _therapyInformationViewModel = therapyInformationViewModel;
        _examinedPatient = therapyInformationViewModel.ExaminedPatient;
        _medicationId = therapyInformationViewModel.MedicationID;
    }

    public override void Execute(object parameter)
    {
        try
        {
            Validate();
            MakeReferral();
        }
        catch (ValidationException ve)
        {
            ViewUtil.ShowWarning(ve.Message);
        }
    }

    private void MakeReferral()
    {
        var dailyDosage = _therapyInformationViewModel.DailyDosage;
        var hoursBetweenConsumption = _therapyInformationViewModel.HoursBetweenConsumption;
        var consumptionDays = _therapyInformationViewModel.ConsumptionDays;
        var doctorJMBG = Context.Current.JMBG;
        var mealTime = GetMealTime();

        var prescription = new Prescription(_medicationId, mealTime, _examinedPatient.JMBG, doctorJMBG, dailyDosage,
            hoursBetweenConsumption, consumptionDays);
        _prescriptionService.Add(prescription);
        _therapyInformationViewModel.Therapy.InitialMedication.Add(prescription.Id);
        _window.Close();
    }

    private MealTime GetMealTime()
    {
        if (_therapyInformationViewModel.BeforeMeal) return MealTime.BeforeMeal;

        if (_therapyInformationViewModel.AfterMeal) return MealTime.AfterMeal;

        if (_therapyInformationViewModel.DuringMeal) return MealTime.DuringMeal;

        return MealTime.NoPreference;
    }

    private void Validate()
    {
        if (_therapyInformationViewModel.DailyDosage <= 0)
            throw new ValidationException("Dnevna doza unosa nije validna");

        if (_therapyInformationViewModel.HoursBetweenConsumption <= 0)
            throw new ValidationException("Sati izmedju konzumacije nisu validni");

        if (_therapyInformationViewModel.HoursBetweenConsumption * _therapyInformationViewModel.DailyDosage > 23)
            throw new ValidationException("Ne ispravan unos vremena i leka na dnevnom nivou");

        if (_therapyInformationViewModel.ConsumptionDays <= 0)
            throw new ValidationException("Broj dana konzumacije nije validan");
    }
}