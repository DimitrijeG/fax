using System.Collections.Generic;
using System.Linq;
using LibraryCirculation.Core.Common;
using LibraryCirculation.DataManagement.Repository;

namespace LibraryCirculation.Core.Fund
{
    public class CopyService : CrudService<Copy>
    {
        public CopyService(IRepository<Copy> repository) : base(repository)
        {
        }

        public new void Add(Copy item)
        {
            if (item.InventoryNumber == 0)
                item.InventoryNumber = GetNextInventoryNumber(item.InventoryBookId);

            if (ContainsInventoryNumber(item.InventoryBookId, item.InventoryNumber))
                return;
            base.Add(item);
        }

        public new void Update(Copy item)
        {
            if (ContainsInventoryNumber(item.InventoryBookId, item.InventoryNumber))
                return;
            base.Update(item);
        }

        private int GetNextInventoryNumber(int inventoryBookId)
        {
            var i = 1;
            while (ContainsInventoryNumber(inventoryBookId, i)) ++i;
            return i;
        }

        private bool ContainsInventoryNumber(int inventoryBookId, int inventoryNumber)
        {
            return GetAll()
                .Where(c => c.InventoryBookId == inventoryBookId)
                .Select(c => c.InventoryNumber)
                .Contains(inventoryNumber);
        }

        public List<Copy> GetForBook(string bookIsbn)
        {
            return GetAll().Where(c => c.BookIsbn == bookIsbn).ToList();
        }
    }
}