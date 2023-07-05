using System.Windows.Input;
using HealthCare.Core.PhysicalAssets;
using HealthCare.WPF.Common;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.Examination.Command;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Examination
{
    public class EquipmentDTO : ViewModelBase
    {
        private readonly Equipment _equipment;
        private readonly InventoryItem _inventoryItem;
        private int _currentQuantity;

        public EquipmentDTO(Equipment equipment, InventoryItem inventoryItem)
        {
            _equipment = equipment;
            _inventoryItem = inventoryItem;
            _currentQuantity = _inventoryItem.Quantity;
            UseEquipment = new QuantityChangeCommand(this);
        }

        public ICommand UseEquipment { get; }

        public string EquipmentName => _equipment.Name;
        public int EquipmentId => _equipment.Id;
        public int InventoryId => _inventoryItem.Id;

        public int CurrentQuantity
        {
            get => _currentQuantity;
            set
            {
                _currentQuantity = value;
                OnPropertyChanged();
            }
        }
    }
}