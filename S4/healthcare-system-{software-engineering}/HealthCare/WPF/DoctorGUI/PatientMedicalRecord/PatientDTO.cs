using HealthCare.Core.Users.Model;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.DoctorGUI.PatientMedicalRecord;

public class PatientDTO : ViewModelBase
{
    private readonly Patient _patient;

    public PatientDTO(Patient patient)
    {
        _patient = patient;
    }

    public string JMBG => _patient.JMBG;
    public string NameAndLastName => _patient.Name + " " + _patient.LastName;
    public string Birthday => ViewUtil.ToString(_patient.BirthDate);
    public string Gender => ViewUtil.Translate(_patient.Gender);
}