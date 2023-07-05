using LibraryCirculation.Core.Common;
using LibraryCirculation.DataManagement.Repository;

namespace LibraryCirculation.Core.Users.Readers
{
    public class MembershipService : CrudService<Membership>
    {
        public MembershipService(IRepository<Membership> repository) : base(repository)
        {
        }
    }
}