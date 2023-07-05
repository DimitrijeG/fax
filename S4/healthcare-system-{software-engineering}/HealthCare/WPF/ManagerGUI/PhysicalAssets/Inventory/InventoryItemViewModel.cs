using System.Windows.Media;
using HealthCare.Core.Interior;
using HealthCare.Core.PhysicalAssets;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.ManagerGUI.PhysicalAssets.Inventory
{
    public class InventoryItemViewModel
    {
        private readonly InventoryItem _item;
        public readonly Equipment Equipment;
        public readonly Room Room;

        public InventoryItemViewModel(InventoryItem item, Equipment equipment, Room room)
        {
            _item = item;
            Equipment = equipment;
            Room = room;
        }

        public string EquipmentName => Equipment.Name;
        public string EquipmentType => ViewUtil.Translate(Equipment.Type);
        public string RoomName => Room.Name;
        public string RoomType => ViewUtil.Translate(Room.Type);
        public int Quantity => _item.Quantity;
        public string IsDynamic => ViewUtil.Translate(Equipment.IsDynamic);
        public Brush Color => Quantity < 5 ? Brushes.Red : Brushes.Black;
    }
}