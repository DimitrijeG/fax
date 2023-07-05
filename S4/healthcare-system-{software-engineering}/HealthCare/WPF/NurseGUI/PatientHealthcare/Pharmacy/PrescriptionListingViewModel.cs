using System.Collections.ObjectModel;
using HealthCare.Application;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.Core.PhysicalAssets;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;

namespace HealthCare.WPF.NurseGUI.PatientHealthcare.Pharmacy
{
    public class PrescriptionListingViewModel
    {
        private readonly DoctorService _doctorService;
        private readonly InventoryService _inventoryService;
        private readonly MedicationService _medicationService;
        private readonly PrescriptionService _prescriptionService;
        private Patient _patient;

        public PrescriptionListingViewModel(Patient patient)
        {
            Prescriptions = new ObservableCollection<PrescriptionViewModel>();

            _prescriptionService = Injector.GetService<PrescriptionService>(Injector.REGULAR_PRESCRIPTION_S);
            _medicationService = Injector.GetService<MedicationService>();
            _inventoryService = Injector.GetService<InventoryService>(Injector.MEDICATION_INVENTORY_S);
            _doctorService = Injector.GetService<DoctorService>();

            _patient = patient;
        }

        public ObservableCollection<PrescriptionViewModel> Prescriptions { get; set; }

        public void Update()
        {
            Prescriptions.Clear();
            foreach (Prescription prescription in _prescriptionService.GetPatientsPrescriptions(_patient.JMBG))
            {
                Doctor doctor = _doctorService.Get(prescription.DoctorJMBG);
                Prescriptions.Add(new PrescriptionViewModel(prescription,
                    _inventoryService.GetTotalQuantity(prescription.MedicationId),
                    _medicationService.Get(prescription.MedicationId).Name,
                    doctor.Name + " " + doctor.LastName));
            }
        }
    }
}