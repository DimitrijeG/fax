using System;
using System.Collections.Generic;
using System.Linq;
using HealthCare.Application;
using HealthCare.Core.Interior.Renovation.Model;
using HealthCare.Core.PhysicalAssets;
using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Interior.Renovation.Service
{
    public class SplittingRenovationService : NumericService<SplittingRenovation>, IRenovationService
    {
        private readonly InventoryService _inventory;
        private readonly RoomService _roomService;

        public SplittingRenovationService(IRepository<SplittingRenovation> repository) : base(repository)
        {
            _roomService = Injector.GetService<RoomService>();
            _inventory = Injector.GetService<InventoryService>(Injector.EQUIPMENT_INVENTORY_S);
            ExecuteAll();
        }

        public IEnumerable<RenovationBase> GetRenovations()
        {
            return GetAll().Cast<RenovationBase>();
        }

        public void Execute(SplittingRenovation renovation)
        {
            var items = _inventory.GetRoomItems(renovation.RoomId);
            int warehouseId = _roomService.GetWarehouseId();
            items.ForEach(x =>
            {
                _inventory.Remove(x.Key);
                x.RoomId = warehouseId;
                _inventory.RestockInventoryItem(x);
            });

            _roomService.Add(renovation.ResultRoom1);
            _roomService.Add(renovation.ResultRoom2);
            _roomService.Remove(renovation.RoomId);

            renovation.Executed = true;
            Update(renovation);
        }

        public void ExecuteAll()
        {
            GetAll().ForEach(x =>
            {
                if (!x.Executed && x.Scheduled.End <= DateTime.Now)
                    Execute(x);
            });
        }
    }
}