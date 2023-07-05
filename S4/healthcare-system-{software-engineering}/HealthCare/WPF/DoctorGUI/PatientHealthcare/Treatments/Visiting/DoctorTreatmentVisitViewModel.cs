using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using HealthCare.Application;
using HealthCare.Core.PatientHealthcare.HealthcareTreatment;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicalPrescription;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicationTherapy.Command;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.Treatments.Visiting.Command;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Treatments.Visiting
{
    public class DoctorTreatmentVisitViewModel : ViewModelBase
    {
        private readonly Patient _patient;
        private readonly PrescriptionService _prescriptionService;
        private readonly Therapy _therapy;
        private readonly Treatment _treatment;
        private readonly TreatmentReferral _treatmentReferral;
        private readonly VisitService _visitService;
        private DateTime _end;

        private ObservableCollection<TherapyPrescriptionDTO> _therapyMedications;
        private ObservableCollection<VisitDTO> _visits;

        public DoctorTreatmentVisitViewModel(Window window, Treatment treatment)
        {
            _prescriptionService = Injector.GetService<PrescriptionService>(Injector.THERAPY_PRESCRIPTION_S);
            _treatment = treatment;
            _treatmentReferral = Injector.GetService<TreatmentReferralService>().Get(treatment.ReferralId);
            _patient = Injector.GetService<PatientService>().Get(_treatmentReferral.PatientJMBG);
            _therapy = Injector.GetService<TherapyService>().Get(_treatmentReferral.TherapyID);
            _visitService = Injector.GetService<VisitService>();

            LoadInformation(treatment);

            ContinueTreatmentCommand = new IncreaseTreatmentDurationCommand(this, _treatment);
            RemoveMedicationFromTherapyCommand = new RemoveMedicationFromTherapyCommand(this, _therapy);
            AddMedicationToTherapyCommand = new ShowAddTherapyDialogCommand(this, _therapy);
            ReleasePatientCommand = new ReleasePatientCommand(window, _treatment);
            _therapyMedications = new ObservableCollection<TherapyPrescriptionDTO>();
            _visits = new ObservableCollection<VisitDTO>();
            Update();
            UpdateVisitListView();
        }

        public TherapyPrescriptionDTO SelectedPrescription { get; set; }
        public string PatientJMBG { get; set; }
        public string PatientNameAndLastName { get; set; }
        public DateTime Start { get; set; }

        public DateTime End
        {
            get => _end;
            set
            {
                _end = value;
                OnPropertyChanged(nameof(End));
            }
        }

        public string AdditionalExamination { get; set; }
        public int DurationIncreaseDays { get; set; }
        public IEnumerable<TherapyPrescriptionDTO> TherapyMedication => _therapyMedications;
        public IEnumerable<VisitDTO> Visits => _visits;

        public ICommand ReleasePatientCommand { get; }
        public ICommand ContinueTreatmentCommand { get; }
        public ICommand AddMedicationToTherapyCommand { get; }
        public ICommand RemoveMedicationFromTherapyCommand { get; }

        public void LoadInformation(Treatment treatment)
        {
            Start = treatment.TreatmentDuration.Start;
            End = treatment.TreatmentDuration.End;
            PatientJMBG = _treatmentReferral.PatientJMBG;
            PatientNameAndLastName = _patient.Name + " " + _patient.LastName;
            AdditionalExamination = ViewUtil.ToString(_treatmentReferral.AdditionalExamination);
        }

        public void Update()
        {
            _therapyMedications.Clear();
            foreach (var prescriptionID in _therapy.InitialMedication)
            {
                Prescription prescription = _prescriptionService.Get(prescriptionID);
                _therapyMedications.Add(new TherapyPrescriptionDTO(prescription));
            }
        }

        private void UpdateVisitListView()
        {
            _visits.Clear();
            foreach (var visit in _visitService.GetVisitsForTreatment(_treatment.Id))
            {
                _visits.Add(new VisitDTO(visit));
            }
        }
    }
}