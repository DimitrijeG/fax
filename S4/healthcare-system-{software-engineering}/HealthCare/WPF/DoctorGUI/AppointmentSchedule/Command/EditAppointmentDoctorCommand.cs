using System.ComponentModel.DataAnnotations;
using HealthCare.Application;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.AppointmentSchedule.Command;

public class EditAppointmentDoctorCommand : CommandBase
{
    private readonly AppointmentService _appointmentService;
    private readonly DoctorMainViewModel _doctorMainViewModel;

    public EditAppointmentDoctorCommand(DoctorMainViewModel viewModel)
    {
        _doctorMainViewModel = viewModel;
        _appointmentService = Injector.GetService<AppointmentService>();
    }

    public override void Execute(object parameter)
    {
        try
        {
            Validate();
            EditSelectedAppointment();
        }
        catch (ValidationException ve)
        {
            ViewUtil.ShowWarning(ve.Message);
        }
    }

    private void EditSelectedAppointment()
    {
        var appointmentViewModel = _doctorMainViewModel.SelectedAppointment;
        var selectedAppointment = _appointmentService.Get(appointmentViewModel.AppointmentID);
        new MakeAppointmentView(_doctorMainViewModel, selectedAppointment).ShowDialog();
    }

    private void Validate()
    {
        var appointmentViewModel = _doctorMainViewModel.SelectedAppointment;
        if (appointmentViewModel == null) throw new ValidationException("Morate odabrati pregled/operaciju iz tabele!");

        var selectedAppointment = _appointmentService.Get(appointmentViewModel.AppointmentID);
        if (selectedAppointment == null) throw new ValidationException("Ups doslo je do greske");
    }
}