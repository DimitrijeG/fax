using System.Windows;
using HealthCare.Application;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Examination.Command;

public class FinishExaminationCommand : CommandBase
{
    private readonly AnamnesisService _anamnesisService;
    private readonly Appointment _appointment;
    private readonly int _roomId;
    private readonly DoctorExamViewModel _viewModel;
    private readonly Window _window;

    public FinishExaminationCommand(Window window, Appointment appointment, DoctorExamViewModel viewModel)
    {
        _anamnesisService = Injector.GetService<AnamnesisService>();
        _viewModel = viewModel;
        _window = window;
        _appointment = appointment;
        _roomId = appointment.RoomID;
    }

    public override void Execute(object parameter)
    {
        _window.Close();

        var conclusion = _viewModel.Conclusion;
        var anamnesis = _anamnesisService.Get(_appointment.AnamnesisID);
        anamnesis.DoctorsObservations = conclusion;
        _anamnesisService.Update(anamnesis);

        new UsedDynamicEquipmentView(_roomId).Show();
    }
}