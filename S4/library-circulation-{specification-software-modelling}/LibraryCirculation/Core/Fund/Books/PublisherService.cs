using LibraryCirculation.Core.Common;
using LibraryCirculation.DataManagement.Repository;

namespace LibraryCirculation.Core.Fund.Books
{
    public class PublisherService : CrudService<Publisher>
    {
        public PublisherService(IRepository<Publisher> repository) : base(repository)
        {
        }
    }
}