using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HealthCare.Application;
using HealthCare.Core.PhysicalAssets;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.ManagerGUI.PhysicalAssets.Orders
{
    public class EquipmentOrderViewModel : ViewModelBase
    {
        private readonly EquipmentService _equipmentService;
        private readonly InventoryService _inventoryService;

        public EquipmentOrderViewModel()
        {
            _inventoryService = Injector.GetService<InventoryService>(Injector.EQUIPMENT_INVENTORY_S);
            _equipmentService = Injector.GetService<EquipmentService>();

            Items = new ObservableCollection<OrderItemViewModel>();
            LoadAll();
        }

        public ObservableCollection<OrderItemViewModel> Items { get; }

        public void LoadAll()
        {
            Items.Clear();
            var items = new List<OrderItemViewModel>();
            foreach (int id in _inventoryService.GetLowQuantityEquipment())
            {
                var equipment = _equipmentService.Get(id);
                if (!equipment.IsDynamic)
                    continue;

                var quantity = _inventoryService.GetTotalQuantity(id);
                items.Add(new OrderItemViewModel(equipment, quantity));
            }

            Sort(items).ForEach(x => Items.Add(x));
        }

        public List<OrderItemViewModel> Sort(List<OrderItemViewModel> items)
        {
            return items.OrderBy(x => x.CurrentQuantity).ToList();
        }
    }
}