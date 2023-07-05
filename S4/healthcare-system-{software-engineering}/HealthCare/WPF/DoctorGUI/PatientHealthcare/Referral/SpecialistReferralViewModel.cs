using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HealthCare.Application;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.Referral.Command;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Referral;

public class SpecialistReferralViewModel : ViewModelBase
{
    private readonly ObservableCollection<DoctorsDTO> _doctors;
    private readonly DoctorService _doctorService;
    public readonly Patient ExaminedPatient;
    private bool _isSpecializationReferral;

    private DoctorsDTO _selectedDoctor;
    private string _specialization;

    public SpecialistReferralViewModel(Patient patient)
    {
        _doctorService = Injector.GetService<DoctorService>();
        ExaminedPatient = patient;
        _specialization = "";
        MakeSpecialistReferralCommand = new AddSpecialistReferralCommand(this);


        _doctors = new ObservableCollection<DoctorsDTO>();
        Update();
    }

    public IEnumerable<DoctorsDTO> Doctors => _doctors;

    public string Specialization
    {
        get => _specialization;
        set
        {
            _specialization = value;
            OnPropertyChanged();
        }
    }

    public DoctorsDTO SelectedDoctor
    {
        get => _selectedDoctor;
        set
        {
            _selectedDoctor = value;
            OnPropertyChanged();
        }
    }

    public bool IsSpecializationReferral
    {
        get => _isSpecializationReferral;
        set
        {
            _isSpecializationReferral = value;
            OnPropertyChanged();
        }
    }

    public ICommand MakeSpecialistReferralCommand { get; }

    private void Update()
    {
        _doctors.Clear();
        foreach (var doctor in _doctorService.GetAll()) _doctors.Add(new DoctorsDTO(doctor));
    }
}