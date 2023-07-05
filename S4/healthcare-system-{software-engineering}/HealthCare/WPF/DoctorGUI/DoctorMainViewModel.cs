using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using HealthCare.Application;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.WPF.Common;
using HealthCare.WPF.DoctorGUI.AbsenceRequesting;
using HealthCare.WPF.DoctorGUI.AppointmentSchedule;
using HealthCare.WPF.DoctorGUI.AppointmentSchedule.Command;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.Examination.Command;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.Treatments;
using HealthCare.WPF.DoctorGUI.PatientMedicalRecord.Command;

namespace HealthCare.WPF.DoctorGUI;

public class DoctorMainViewModel : ViewModelBase
{
    private readonly ObservableCollection<AppointmentDTO> _appointments;
    private readonly AppointmentService _appointmentService;
    private int _numberOfDays = 3;
    private AppointmentDTO _selectedAppointment;
    private DateTime _startDate = DateTime.Now;

    public DoctorMainViewModel(Window window)
    {
        _appointments = new ObservableCollection<AppointmentDTO>();

        _appointmentService = Injector.GetService<AppointmentService>();

        ResetFilterCommand = new ResetFilterCommand(this);
        LogOutCommand = new LogOutCommand(window);
        CreateAppointmentViewCommand = new MakeAppointmentNavigationCommand(this);
        EditAppointmentCommand = new EditAppointmentDoctorCommand(this);
        DeleteAppointmentCommand = new DeleteAppointmentCommand(this);
        ShowDetailedPatientInfoCommand = new ShowPatientInfoCommand(this, false);
        ApplyFilterCommand = new ApplyFilterCommand(this);
        ShowPatientSearchCommand = new ShowPatientSearchViewCommand();
        StartExaminationCommand = new StartExaminationCommand(this);
        ShowTreatmentsCommand = new ShowTreatmentCommand();
        ShowAbsenceRequestViewCommand = new ShowAbsenceRequestViewCommand();

        Update();
    }

    public IEnumerable<AppointmentDTO> Appointments => _appointments;
    public ICommand CreateAppointmentViewCommand { get; }
    public ICommand EditAppointmentCommand { get; }
    public ICommand DeleteAppointmentCommand { get; }
    public ICommand ShowDetailedPatientInfoCommand { get; }
    public ICommand ApplyFilterCommand { get; }
    public ICommand ShowPatientSearchCommand { get; }
    public ICommand StartExaminationCommand { get; }
    public ICommand ResetFilterCommand { get; }
    public ICommand ShowTreatmentsCommand { get; }
    public ICommand LogOutCommand { get; }
    public ICommand ShowAbsenceRequestViewCommand { get; }

    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            _startDate = value;
            OnPropertyChanged();
        }
    }

    public int NumberOfDays
    {
        get => _numberOfDays;
        set
        {
            _numberOfDays = value;
            OnPropertyChanged();
        }
    }

    public AppointmentDTO SelectedAppointment
    {
        get => _selectedAppointment;
        set
        {
            _selectedAppointment = value;
            OnPropertyChanged();
        }
    }

    public void ApplyFilterOnAppointments(List<Appointment> appointments)
    {
        _appointments.Clear();
        foreach (var appointment in appointments) _appointments.Add(new AppointmentDTO(appointment));
    }

    public void Update()
    {
        _appointments.Clear();
        foreach (var appointment in _appointmentService.GetByDoctor(Context.Current.JMBG))
            _appointments.Add(new AppointmentDTO(appointment));
    }
}