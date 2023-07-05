using System;
using System.Windows;
using System.Windows.Input;
using HealthCare.Core.PatientHealthcare.HealthcareTreatment;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.MedicationTherapy.Command
{
    public class PatientReleaseAppointmentViewModel : ViewModelBase
    {
        private DateTime _date;
        private int _hours;
        private int _minutes;

        public PatientReleaseAppointmentViewModel(Window window, Treatment treatment)
        {
            _date = DateTime.Today.AddDays(10);
            MakeAppointmentCommand = new MakeAppointmentFromReleaseCommand(this, window, treatment);
        }

        public int Minutes
        {
            get => _minutes;
            set
            {
                if (value > 59 || value < 0)
                    _minutes = 0;
                else
                    _minutes = value;
                OnPropertyChanged(nameof(Minutes));
            }
        }

        public int Hours
        {
            get => _hours;
            set
            {
                if (value > 23 || value < 0)
                    _hours = 0;
                else
                    _hours = value;
                OnPropertyChanged(nameof(Hours));
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public ICommand MakeAppointmentCommand { get; }
    }
}