using System;
using System.Windows;
using HealthCare.Application;
using HealthCare.Core.Scheduling;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Scheduling.Schedules;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.AppointmentSchedule.Command;

public class AddNewAppointmentDoctorCommand : CommandBase
{
    private readonly AppointmentService _appointmentService;
    private readonly DoctorMainViewModel _doctorMainViewModel;
    private readonly bool _isEditing;
    private readonly MakeAppointmentViewModel _makeAppointmentViewModel;
    private readonly PatientService _patientService;
    private readonly Schedule _schedule;
    private readonly Window _window;

    public AddNewAppointmentDoctorCommand(MakeAppointmentViewModel viewModel, DoctorMainViewModel docMainViewModel,
        Window window, bool isEditing)
    {
        _makeAppointmentViewModel = viewModel;
        _doctorMainViewModel = docMainViewModel;
        _window = window;
        _isEditing = isEditing;
        _appointmentService = Injector.GetService<AppointmentService>();
        _patientService = Injector.GetService<PatientService>();
        _schedule = Injector.GetService<Schedule>();
    }

    public override void Execute(object parameter)
    {
        if (_makeAppointmentViewModel.SelectedPatient is null)
        {
            ViewUtil.ShowWarning("Morate odabrati pacijenta!");
            return;
        }

        if (GetPatient(out var patient)) return;

        CommitChanges(patient);
    }

    private bool GetPatient(out Patient? patient)
    {
        patient = _patientService.TryGet(_makeAppointmentViewModel.SelectedPatient.JMBG);
        if (patient is not null) return false;
        ViewUtil.ShowError("Oops... Doslo je do greske probajte ponovo!");
        return true;
    }

    private void CommitChanges(Patient patient)
    {
        var newAppointment = MakeAppointment(patient);

        if (_isEditing)
        {
            EditAppointment(newAppointment);
        }
        else
        {
            if (!_schedule.IsAvailable(newAppointment))
            {
                ViewUtil.ShowWarning("Doktor ili pacijent je zauzet u ovom terminu, odaberite drugi termin");
                return;
            }

            _schedule.Add(newAppointment);
            _doctorMainViewModel.Update();
            _window.Close();
        }
    }

    private void EditAppointment(Appointment newAppointment)
    {
        newAppointment.AppointmentID = Convert.ToInt32(_doctorMainViewModel.SelectedAppointment.AppointmentID);
        newAppointment.RoomID = Injector.GetService<AppointmentService>().Get(newAppointment.AppointmentID).RoomID;
        if (!_schedule.IsAvailable(newAppointment))
        {
            ViewUtil.ShowWarning("Doktor, pacijent ili soba su zauzeti u ovom terminu, odaberite drugi termin");
            return;
        }

        _appointmentService.Update(newAppointment);
        _doctorMainViewModel.Update();
        _window.Close();
    }

    private Appointment MakeAppointment(Patient patient)
    {
        var start = _makeAppointmentViewModel.StartDate.Date +
                    new TimeSpan(_makeAppointmentViewModel.Hours, _makeAppointmentViewModel.Minutes, 0);
        var duration =
            TimeSpan.FromMinutes(_makeAppointmentViewModel.IsOperation ? _makeAppointmentViewModel.Duration : 15);
        var timeSlot = new TimeSlot(start, duration);
        var newAppointment = new Appointment(patient.JMBG, Context.Current.JMBG, timeSlot,
            _makeAppointmentViewModel.IsOperation);
        return newAppointment;
    }
}