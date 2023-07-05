using System;
using System.Windows;
using System.Windows.Controls;
using HealthCare.Application;
using HealthCare.Core.Scheduling;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Scheduling.Schedules;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.NurseGUI.PatientHealthcare.Referrals.Specialist
{
    public partial class SpecialistReferralsView
    {
        private readonly DoctorService _doctorService;
        private readonly Schedule _schedule;
        private readonly SpecialistReferralService _specialistReferralService;
        private SpecialistReferralListingViewModel _model;
        private Patient _patient;
        private SpecialistReferralViewModel? _referral;

        public SpecialistReferralsView(Patient patient)
        {
            InitializeComponent();
            _model = new SpecialistReferralListingViewModel(patient);
            DataContext = _model;

            _specialistReferralService = Injector.GetService<SpecialistReferralService>();
            _doctorService = Injector.GetService<DoctorService>();
            _schedule = Injector.GetService<Schedule>();

            _patient = patient;
            tbDate.SelectedDate = DateTime.Now;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void lvReferrals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _referral = (SpecialistReferralViewModel)lvReferrals.SelectedItem;
        }

        private void btnMakeAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (_referral is null)
            {
                ViewUtil.ShowWarning("Izaberite upit koji zelite da iskoristite.");
                return;
            }

            Doctor referred = _doctorService.Get(_referral.SpecialistReferral.ReferredDoctorJMBG);

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
            Appointment appointment = new Appointment(_patient.JMBG, referred.JMBG, slot, false);

            if (!_schedule.IsAvailable(appointment))
            {
                ViewUtil.ShowWarning("Doktor ili pacijent je zauzet u unetom terminu.");
                return;
            }

            _schedule.Add(appointment);
            ViewUtil.ShowInformation("Uspesno ste zakazali pregled.");

            var updated = _specialistReferralService.Get(_referral.SpecialistReferral.Id);
            updated.IsUsed = true;
            _specialistReferralService.Update(updated);

            _model.Update();
        }
    }
}