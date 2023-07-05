using System.Collections.Generic;
using System.Linq;
using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.PhysicalAssets
{
    public class InventoryService : NumericService<InventoryItem>
    {
        public InventoryService(IRepository<InventoryItem> repository) : base(repository)
        {
        }

        public int GetTotalQuantity(int equipmentId)
        {
            return GetEquipmentItems(equipmentId).Sum(x => x.Quantity);
        }

        public IEnumerable<int> GetLowQuantityEquipment(int threshold = 200)
        {
            return GetAll()
                .GroupBy(x => x.ItemId)
                .Where(group => group.Sum(x => x.Quantity) < threshold)
                .Select(group => group.Key);
        }

        public void RestockInventoryItem(InventoryItem item)
        {
            var found = GetAll().Find(x =>
                x.ItemId == item.ItemId && x.RoomId == item.RoomId);

            if (found is not null)
            {
                found.Quantity += item.Quantity;
                Update(found);
            }
            else
                Add(item);
        }

        public bool TryReduceInventoryItem(InventoryItem item)
        {
            var a = GetAll();
            var found = GetAll().Find(x =>
                x.ItemId == item.ItemId && x.RoomId == item.RoomId);

            if (found is null || found.Quantity < item.Quantity)
                return false;

            found.Quantity -= item.Quantity;
            if (found.Quantity == 0)
                Remove(found.Key);
            else
                Update(found);
            return true;
        }

        private List<InventoryItem> GetEquipmentItems(int equipmentId)
        {
            return GetAll().Where(x => x.ItemId == equipmentId).ToList();
        }

        public List<InventoryItem> GetRoomItems(int roomId)
        {
            return GetAll().Where(x => x.RoomId == roomId).ToList();
        }

        public void ChangeDynamicEquipmentQuantity(Dictionary<int, int> newQuantities)
        {
            foreach (KeyValuePair<int, int> entry in newQuantities)
            {
                InventoryItem item = Get(entry.Key);
                item.Quantity = entry.Value;
                Update(item);
            }
        }

        public InventoryItem? SearchByEquipmentAndRoom(int equipmentId, int roomId)
        {
            return GetAll().Find(x => x.ItemId == equipmentId && x.RoomId == roomId);
        }

        internal List<InventoryItem> CombineItems(IEnumerable<InventoryItem> items1, IEnumerable<InventoryItem> items2)
        {
            return items1.Union(items2)
                .GroupBy(x => x.ItemId)
                .Select(g => new InventoryItem(g.Key, 0, g.Sum(x => x.Quantity)))
                .ToList();
        }
    }
}