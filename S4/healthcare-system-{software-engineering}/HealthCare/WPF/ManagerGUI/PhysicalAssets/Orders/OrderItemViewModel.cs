using HealthCare.Core.PhysicalAssets;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.ManagerGUI.PhysicalAssets.Orders
{
    public class OrderItemViewModel : ViewModelBase
    {
        private readonly Equipment _equipment;
        private bool _isSelected;

        private string _orderQuantity;

        public OrderItemViewModel(Equipment equipment, int currentQuantity)
        {
            _equipment = equipment;
            _isSelected = false;
            CurrentQuantity = currentQuantity;
            _orderQuantity = "0";
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public string EquipmentName => _equipment.Name;
        public string EquipmentType => ViewUtil.Translate(_equipment.Type);
        public int EquipmentId => _equipment.Id;
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