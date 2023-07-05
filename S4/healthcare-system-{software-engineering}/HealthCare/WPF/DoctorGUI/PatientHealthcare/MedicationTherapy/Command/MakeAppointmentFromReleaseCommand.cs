using System;
using System.Windows;
using HealthCare.Application;
using HealthCare.Application.Exceptions;
using HealthCare.Core.PatientHealthcare.HealthcareTreatment;
using HealthCare.Core.Scheduling;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Scheduling.Schedules;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicationTherapy.Command
{
    public class MakeAppointmentFromReleaseCommand : CommandBase
    {
        private readonly Schedule _schedule;
        private readonly Treatment _treatment;
        private readonly TreatmentReferralService _treatmentReferralService;
        private readonly PatientReleaseAppointmentViewModel _viewModel;
        private readonly Window _window;
        private Appointment _appointment;

        public MakeAppointmentFromReleaseCommand(PatientReleaseAppointmentViewModel viewModel, Window window,
            Treatment treatment)
        {
            _treatmentReferralService = Injector.GetService<TreatmentReferralService>();
            _schedule = Injector.GetService<Schedule>();
            _window = window;
            _viewModel = viewModel;
            _treatment = treatment;
        }

        public override void Execute(object parameter)
        {
            try
            {
                Validate();
                MakeAppointment();
                _window.Close();
            }
            catch (ValidationException ve)
            {
                ViewUtil.ShowWarning(ve.Message);
            }
        }

        private void Validate()
        {
            DateTime date = _viewModel.Date;

            if (date < DateTime.Now)
            {
                throw new ValidationException("Datum mora biti u buducnosti");
            }

            TimeSpan appointmentDuration = new TimeSpan(0, 15, 0);
            TreatmentReferral referral = _treatmentReferralService.Get(_treatment.ReferralId);
            date = date.AddHours(_viewModel.Hours).AddMinutes(_viewModel.Minutes);
            Appointment appointment = new Appointment(referral.PatientJMBG, referral.DoctorJMBG,
                new TimeSlot(date, appointmentDuration), false);
            if (!_schedule.IsAvailable(appointment))
            {
                throw new ValidationException("Nazalost uneti termin nije slobodan");
            }

            _appointment = appointment;
        }

        private void MakeAppointment()
        {
            _schedule.Add(_appointment);
        }
    }
}