using System;
using HealthCare.Application;
using HealthCare.Application.Exceptions;
using HealthCare.Core.HumanResources;
using HealthCare.Core.Scheduling;
using HealthCare.Core.Scheduling.Schedules;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.AbsenceRequesting
{
    public class MakeAbsenceRequestCommand : CommandBase
    {
        private readonly AbsenceRequestService _absenceRequestService;
        private readonly AbsenceRequestViewModel _absenceRequestViewModel;
        private readonly DoctorSchedule _doctorSchedule;

        public MakeAbsenceRequestCommand(AbsenceRequestViewModel absenceRequestViewModel)
        {
            _absenceRequestViewModel = absenceRequestViewModel;
            _absenceRequestService = Injector.GetService<AbsenceRequestService>();
            _doctorSchedule = Injector.GetService<DoctorSchedule>();
        }

        public override void Execute(object parameter)
        {
            try
            {
                Validate();
                MakeAbsenceRequest();
                _absenceRequestViewModel.Update();
            }
            catch (ValidationException ve)
            {
                ViewUtil.ShowWarning(ve.Message);
            }
        }

        private void Validate()
        {
            DateTime startDate = _absenceRequestViewModel.AbsenceStartingDate;
            int absenceDays = _absenceRequestViewModel.AbsenceDurationDays;
            TimeSpan duration = new TimeSpan(absenceDays, 0, 0, 0);

            if (!_doctorSchedule.IsAvailable(Context.Current.JMBG, new TimeSlot(startDate, duration)))
            {
                throw new ValidationException("Zauzeti ste u datom terminu");
            }

            if (!(startDate > DateTime.Today.AddDays(2)))
            {
                throw new ValidationException("Morate podneti zahtev minimum 2 dana ranije");
            }

            if (_absenceRequestViewModel.AbsenceDurationDays == 0)
            {
                throw new ValidationException("Broj dana mora biti vise od nula");
            }
        }

        private void MakeAbsenceRequest()
        {
            string doctorJMBG = Context.Current.JMBG;
            string reason = _absenceRequestViewModel.AbsenceReason;
            int absenceDays = _absenceRequestViewModel.AbsenceDurationDays;
            TimeSpan duration = new TimeSpan(absenceDays, 0, 0, 0);
            DateTime startDate = _absenceRequestViewModel.AbsenceStartingDate;

            AbsenceRequest absenceRequest = new AbsenceRequest(doctorJMBG, reason, new TimeSlot(startDate, duration));
            _absenceRequestService.Add(absenceRequest);
        }
    }
}