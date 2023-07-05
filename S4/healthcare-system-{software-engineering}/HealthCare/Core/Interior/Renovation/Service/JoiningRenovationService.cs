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
    public class JoiningRenovationService : NumericService<JoiningRenovation>, IRenovationService
    {
        private readonly InventoryService _inventory;
        private readonly RoomService _roomService;

        public JoiningRenovationService(IRepository<JoiningRenovation> repository) : base(repository)
        {
            _inventory = Injector.GetService<InventoryService>(Injector.EQUIPMENT_INVENTORY_S);
            _roomService = Injector.GetService<RoomService>();
            ExecuteAll();
        }

        public IEnumerable<RenovationBase> GetRenovations()
        {
            return GetAll().Cast<RenovationBase>();
        }

        public void Execute(JoiningRenovation renovation)
        {
            var items1 = _inventory.GetRoomItems(renovation.RoomId);
            var items2 = _inventory.GetRoomItems(renovation.OtherRoomId);

            var combined = _inventory.CombineItems(items1, items2);
            int newId = _roomService.Add(renovation.ResultRoom);

            items1.ForEach(x => _inventory.Remove(x.Id));
            items2.ForEach(x => _inventory.Remove(x.Id));
            combined.ForEach(x =>
            {
                x.RoomId = newId;
                _inventory.RestockInventoryItem(x);
            });

            _roomService.Remove(renovation.RoomId);
            _roomService.Remove(renovation.OtherRoomId);

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