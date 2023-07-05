using System.Collections.Generic;
using System.Linq;
using LibraryCirculation.Core.Common;
using LibraryCirculation.DataManagement.Repository;

namespace LibraryCirculation.Core.Renting
{
    public class RentalService : CrudService<Rental>
    {
        public RentalService(IRepository<Rental> repository) : base(repository)
        {
        }

        public List<int> GetRentedCopyIds()
        {
            return GetAll().Where(r => r.Returned = false).Select(r => r.CopyId).ToList();
        }
    }
}