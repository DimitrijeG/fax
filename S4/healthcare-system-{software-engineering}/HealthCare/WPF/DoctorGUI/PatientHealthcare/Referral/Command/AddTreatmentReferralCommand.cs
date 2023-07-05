using System.Collections.Generic;
using HealthCare.Application;
using HealthCare.Application.Exceptions;
using HealthCare.Core.PatientHealthcare.HealthcareTreatment;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.Core.Users.Model;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicationTherapy;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Referral.Command;

public class AddTreatmentReferralCommand : CommandBase
{
    private readonly Patient _examinedPatient;
    private readonly MedicationService _medicationService;
    private readonly TherapyService _therapyService;
    private readonly TreatmentReferralService _treatmentReferralService;
    private readonly TreatmentReferralViewModel _treatmentReferralViewModel;

    public AddTreatmentReferralCommand(TreatmentReferralViewModel treatmentReferralViewModel)
    {
        _treatmentReferralService = Injector.GetService<TreatmentReferralService>();
        _medicationService = Injector.GetService<MedicationService>();
        _therapyService = Injector.GetService<TherapyService>();

        _treatmentReferralViewModel = treatmentReferralViewModel;
        _examinedPatient = treatmentReferralViewModel.ExaminedPatient;
    }

    public override void Execute(object parameter)
    {
        try
        {
            MakeReferral();
            ViewUtil.ShowInformation("Uput za bolnicko lecenje uspesno dodat");
        }
        catch (ValidationException ex)
        {
            ViewUtil.ShowWarning(ex.Message);
        }
    }

    private void MakeReferral()
    {
        var medication = GetMedication();
        var daysOfTreatment = _treatmentReferralViewModel.DaysOfTreatment;
        var doctorJMBG = Context.Current.JMBG;
        var additionalExamination = ViewUtil.GetStringList(_treatmentReferralViewModel.AdditionalExamination);

        CheckPatientAllergies(_examinedPatient, medication);

        var therapyId = GetTherapy(medication);

        var treatmentReferral = new TreatmentReferral(daysOfTreatment, _examinedPatient.JMBG, doctorJMBG, therapyId,
            additionalExamination);
        _treatmentReferralService.Add(treatmentReferral);
    }

    private List<int> GetMedication()
    {
        var selectedMedication = new List<int>();
        foreach (var medication in _treatmentReferralViewModel.Medications)
            if (medication.InitialTherapy)
                selectedMedication.Add(medication.MedicationId);
        return selectedMedication;
    }

    private void CheckPatientAllergies(Patient patient, List<int> therapy)
    {
        foreach (var MedicationID in therapy)
        {
            var medication = _medicationService.Get(MedicationID);

            if (patient.IsAllergic(medication.Ingredients))
                throw new ValidationException("Pacijent je alergican na lek: " + medication.Name);
        }
    }

    private int GetTherapy(List<int> medication)
    {
        var therapy = new Therapy(new List<int>(), _examinedPatient.JMBG);

        foreach (var MedicationID in medication)
            new TherapyInformation(_examinedPatient, MedicationID, therapy).ShowDialog();
        _therapyService.Add(therapy);

        return therapy.Id;
    }
}