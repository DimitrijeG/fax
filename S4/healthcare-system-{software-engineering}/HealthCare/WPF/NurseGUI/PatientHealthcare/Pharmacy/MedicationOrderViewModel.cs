using System.Windows.Media;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.NurseGUI.PatientHealthcare.Pharmacy
{
    public class OrderMedicationViewModel : ViewModelBase
    {
        private readonly Medication _medication;
        private bool _isSelected;
        private string _orderQuantity;

        public OrderMedicationViewModel(Medication medication, int currentQuantity)
        {
            _medication = medication;
            _isSelected = false;
            CurrentQuantity = currentQuantity;
            _orderQuantity = "0";
        }

        public Brush Color => CurrentQuantity == 0 ? Brushes.Red : Brushes.Black;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public int Id => _medication.Id;
        public string Name => _medication.Name;

        public int CurrentQuantity { get; }

        public string OrderQuantity
        {
            get => _orderQuantity;
            set
            {
                _orderQuantity = value;
                IsSelected = Validation.IsNatural(value);
            }
        }
    }
}