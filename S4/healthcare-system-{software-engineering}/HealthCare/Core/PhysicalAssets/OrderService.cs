using System;
using HealthCare.Application;
using HealthCare.Core.Interior;
using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.PhysicalAssets
{
    public class OrderService : NumericService<OrderItem>
    {
        private readonly InventoryService _inventory;
        private readonly RoomService _roomService;

        public OrderService(IRepository<OrderItem> repository, InventoryService inventory) : base(repository)
        {
            _roomService = Injector.GetService<RoomService>();
            _inventory = inventory;

            ExecuteAll();
        }

        public void Execute(OrderItem item)
        {
            int warehouseId = _roomService.GetWarehouseId();

            var restockItem = new InventoryItem(
                item.ItemId, warehouseId, item.Quantity);

            _inventory.RestockInventoryItem(restockItem);
            item.Executed = true;

            Update(item);
        }

        public void ExecuteAll()
        {
            GetAll().ForEach(x =>
            {
                if (!x.Executed && x.Scheduled <= DateTime.Now)
                    Execute(x);
            });
        }
    }
}