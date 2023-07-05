using LibraryCirculation.Core.Common;
using LibraryCirculation.DataManagement.Repository;

namespace LibraryCirculation.Core.Finance
{
    public class PaymentService : CrudService<Payment>
    {
        public PaymentService(IRepository<Payment> repository) : base(repository)
        {
        }
    }
}