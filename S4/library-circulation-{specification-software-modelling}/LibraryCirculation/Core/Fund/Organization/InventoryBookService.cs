using System.Collections.Generic;
using System.Linq;
using LibraryCirculation.Core.Common;
using LibraryCirculation.DataManagement.Repository;

namespace LibraryCirculation.Core.Fund.Organization
{
    public class InventoryBookService : CrudService<InventoryBook>
    {
        public InventoryBookService(IRepository<InventoryBook> repository) : base(repository)
        {
        }

        public List<InventoryBook> GetForBranch(int branchId)
        {
            return GetAll().Where(ib => ib.BranchId == branchId).ToList();
        }
    }
}