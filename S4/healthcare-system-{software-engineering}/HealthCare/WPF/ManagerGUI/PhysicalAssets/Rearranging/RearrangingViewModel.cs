using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HealthCare.Application;
using HealthCare.Core.Interior;
using HealthCare.Core.PhysicalAssets;
using HealthCare.WPF.ManagerGUI.PhysicalAssets.Inventory;

namespace HealthCare.WPF.ManagerGUI.PhysicalAssets.Rearranging
{
    public class RearrangingViewModel
    {
        private readonly EquipmentService _equipmentService;
        private readonly InventoryService _inventoryService;
        private readonly RoomService _roomService;

        public RearrangingViewModel()
        {
            _inventoryService = Injector.GetService<InventoryService>(Injector.EQUIPMENT_INVENTORY_S);
            _equipmentService = Injector.GetService<EquipmentService>();
            _roomService = Injector.GetService<RoomService>();
            FromRooms = new ObservableCollection<InventoryItemViewModel>();
            ToRooms = new ObservableCollection<InventoryItemViewModel>();
        }

        public ObservableCollection<InventoryItemViewModel> FromRooms { get; }
        public ObservableCollection<InventoryItemViewModel> ToRooms { get; }
        public List<Equipment> Equipment => _equipmentService.GetAll();

        public void Load(Equipment equipment)
        {
            FromRooms.Clear();
            ToRooms.Clear();
            var fromRooms = new List<InventoryItemViewModel>();
            var toRooms = new List<InventoryItemViewModel>();

            foreach (Room room in _roomService.GetAll())
            {
                var found = _inventoryService.SearchByEquipmentAndRoom(equipment.Id, room.Id);

                if (found is not null && found.Quantity > 0)
                    fromRooms.Add(new InventoryItemViewModel(found, equipment, room));
                else
                    found = new InventoryItem(0, equipment.Id, room.Id, 0);

                toRooms.Add(new InventoryItemViewModel(found, equipment, room));
            }

            Sort(fromRooms, -1).ForEach(model => FromRooms.Add(model));
            Sort(toRooms).ForEach(model => ToRooms.Add(model));
        }

        public List<InventoryItemViewModel> Sort(List<InventoryItemViewModel> items, int order = 1)
        {
            IEnumerable<InventoryItemViewModel> sorted;
            if (order == -1)
                sorted = items.OrderByDescending(x => x.Quantity).ThenBy(x => x.RoomName);
            else
                sorted = items.OrderBy(x => x.Quantity).ThenBy(x => x.RoomName);
            return sorted.ToList();
        }
    }
}