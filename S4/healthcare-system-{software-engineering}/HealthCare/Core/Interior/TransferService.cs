using System;
using HealthCare.Application;
using HealthCare.Core.PhysicalAssets;
using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Interior
{
    public class TransferService : NumericService<TransferItem>
    {
        private readonly InventoryService _inventory;

        public TransferService(IRepository<TransferItem> repository) : base(repository)
        {
            _inventory = Injector.GetService<InventoryService>(Injector.EQUIPMENT_INVENTORY_S);

            ExecuteAll();
        }

        public void Execute(TransferItem transfer)
        {
            var reduceItem = new InventoryItem(
                transfer.ItemId, transfer.FromRoom, transfer.Quantity);
            var restockItem = new InventoryItem(
                transfer.ItemId, transfer.ToRoom, transfer.Quantity);

            if (!_inventory.TryReduceInventoryItem(reduceItem))
                return;

            _inventory.RestockInventoryItem(restockItem);
            transfer.Executed = true;

            if (transfer.Id == 0)
                Add(transfer);
            else
                Update(transfer);
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