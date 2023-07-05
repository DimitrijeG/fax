using LibraryCirculation.Core.Common;
using LibraryCirculation.DataManagement.Repository;

namespace LibraryCirculation.Core.Fund.Organization
{
    public class BranchService : CrudService<Branch>
    {
        public BranchService(IRepository<Branch> repository) : base(repository)
        {
        }
    }
}