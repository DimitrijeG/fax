using HealthCare.Application;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.DoctorGUI.AppointmentSchedule;

public class AppointmentDTO : ViewModelBase
{
    private readonly Appointment _appointment;
    private readonly Doctor _doctor;
    private readonly Patient _patient;

    public AppointmentDTO(Appointment appointment)
    {
        _appointment = appointment;
        _patient = Injector.GetService<PatientService>().Get(appointment.PatientJMBG);
        _doctor = Injector.GetService<DoctorService>().Get(appointment.DoctorJMBG);
    }

    public int AppointmentID => _appointment.AppointmentID;
    public string Patient => _patient.Name + " " + _patient.LastName;
    public string Doctor => _doctor.Name + " " + _doctor.LastName;
    public string StartingTime => _appointment.TimeSlot.Start.ToString();
    public string Duration => _appointment.TimeSlot.Duration.ToString();
    public bool IsOperation => _appointment.IsOperation;
    public string JMBG => _patient.JMBG;
}