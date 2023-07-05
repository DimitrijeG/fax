using System;
using System.Windows;
using System.Windows.Controls;
using HealthCare.Application;
using HealthCare.Core.Interior;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.Core.PhysicalAssets;
using HealthCare.Core.Scheduling;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Scheduling.Schedules;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.NurseGUI.PatientHealthcare.Pharmacy
{
    public partial class PrescriptionsView
    {
        private readonly DoctorService _doctorService;
        private readonly InventoryService _medicationInventory;
        private readonly PrescriptionService _prescriptionService;
        private readonly RoomService _roomService;
        private readonly Schedule _schedule;
        private PrescriptionListingViewModel _model;
        private Patient _patient;
        private PrescriptionViewModel? _prescription;

        public PrescriptionsView(Patient patient)
        {
            InitializeComponent();

            _model = new PrescriptionListingViewModel(patient);
            DataContext = _model;
            _model.Update();

            _prescriptionService = Injector.GetService<PrescriptionService>(Injector.REGULAR_PRESCRIPTION_S);
            _medicationInventory = Injector.GetService<InventoryService>(Injector.MEDICATION_INVENTORY_S);
            _doctorService = Injector.GetService<DoctorService>();
            _roomService = Injector.GetService<RoomService>();
            _schedule = Injector.GetService<Schedule>();

            _patient = patient;
            tbDate.SelectedDate = DateTime.Now;
        }

        private void lvPrescriptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _prescription = (PrescriptionViewModel)lvPrescriptions.SelectedItem;
            if (_prescription is null)
            {
                btnUse.IsEnabled = true;
                btnProlonge.IsEnabled = true;
                return;
            }

            if (_prescription.Prescription.FirstUse)
            {
                btnUse.IsEnabled = true;
                btnProlonge.IsEnabled = false;
            }
            else
            {
                btnUse.IsEnabled = false;
                btnProlonge.IsEnabled = true;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnUse_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
                return;

            Prescription prescription = _prescriptionService.Get(_prescription.Prescription.Id);

            if (!prescription.FirstUse)
            {
                ViewUtil.ShowWarning("Vec ste iskoristili recept.");
                return;
            }

            if (!GiveMedication(prescription))
                return;

            prescription.FirstUse = false;
            prescription.Start = DateTime.Now.Date;
            _prescriptionService.Update(prescription);

            _model.Update();
        }

        private void btnProlonge_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
                return;
            Prescription prescription = _prescriptionService.Get(_prescription.Prescription.Id);

            if (prescription.FirstUse)
            {
                ViewUtil.ShowWarning("Niste iskoristili recept.");
                return;
            }

            if (prescription.Start.AddDays(prescription.ConsumptionDays - 1) > DateTime.Now)
            {
                ViewUtil.ShowWarning("Nije vam isteklo vreme.");
                return;
            }

            if (!GiveMedication(prescription))
                return;

            prescription.Start = DateTime.Now.Date;
            _prescriptionService.Update(prescription);

            _model.Update();
        }

        private void btnMakeAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
                return;

            Doctor doctor = _doctorService.Get(_prescription.Prescription.DoctorJMBG);

            if (!int.TryParse(tbHours.Text, out _) && !int.TryParse(tbMinutes.Text, out _))
            {
                ViewUtil.ShowWarning("Sati i minuti moraju biti brojevi");
                return;
            }

            int hours = int.Parse(tbHours.Text);
            int minutes = int.Parse(tbMinutes.Text);

            if (!tbDate.SelectedDate.HasValue)
            {
                ViewUtil.ShowWarning("Izaberite datum.");
                return;
            }

            DateTime selectedDate = tbDate.SelectedDate.Value;
            selectedDate = selectedDate.AddHours(hours);
            selectedDate = selectedDate.AddMinutes(minutes);

            if (selectedDate <= DateTime.Now)
            {
                ViewUtil.ShowWarning("Datum ne sme da bude u proslosti.");
                return;
            }

            TimeSlot slot = new TimeSlot(selectedDate, new TimeSpan(0, 15, 0));
            Appointment appointment = new Appointment(_patient.JMBG, doctor.JMBG, slot, false);

            if (!_schedule.IsAvailable(appointment))
            {
                ViewUtil.ShowWarning("Doktor ili pacijent je zauzet u unetom terminu.");
                return;
            }

            _schedule.Add(appointment);
            ViewUtil.ShowInformation("Uspesno ste zakazali pregled.");
            _model.Update();
        }

        private bool GiveMedication(Prescription prescription)
        {
            var reduceItem = new InventoryItem(
                prescription.MedicationId, _roomService.GetWarehouseId(), prescription.GetQuantity());

            if (!_medicationInventory.TryReduceInventoryItem(reduceItem))
            {
                ViewUtil.ShowError("Nema lekova na stanju.");
                return false;
            }

            ViewUtil.ShowInformation("Uspesno izdati lekovi.");
            return true;
        }

        private bool Validate()
        {
            if (_prescription is null)
            {
                ViewUtil.ShowWarning("Izaberite recept");
                return false;
            }

            return true;
        }
    }
}