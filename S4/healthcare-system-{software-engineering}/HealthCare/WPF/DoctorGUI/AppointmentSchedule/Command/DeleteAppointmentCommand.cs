using System.ComponentModel.DataAnnotations;
using HealthCare.Application;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.AppointmentSchedule.Command;

internal class DeleteAppointmentCommand : CommandBase
{
    private readonly AppointmentService _appointmentService;
    private readonly DoctorMainViewModel _viewModel;

    public DeleteAppointmentCommand(DoctorMainViewModel mainViewModel)
    {
        _viewModel = mainViewModel;
        _appointmentService = Injector.GetService<AppointmentService>();
    }

    public override void Execute(object parameter)
    {
        try
        {
            Validate();
            var a = _viewModel.SelectedAppointment;
            var appointment = _appointmentService.Get(a.AppointmentID);
            _appointmentService.Remove(appointment.AppointmentID);
            _viewModel.Update();
        }
        catch (ValidationException ve)
        {
            ViewUtil.ShowWarning(ve.Message);
        }
    }

    private void Validate()
    {
        var selectedAppointmentId = _viewModel.SelectedAppointment?.AppointmentID;
        if (selectedAppointmentId is null) throw new ValidationException("Odaberite pregled/operaciju iz tabele!");

        var selectedAppointment = _appointmentService.Get(selectedAppointmentId);
        if (selectedAppointment is null) throw new ValidationException("Ups Doslo je do greske!");
    }
}