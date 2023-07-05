using LibraryCirculation.Core.Common;
using LibraryCirculation.DataManagement.Repository;

namespace LibraryCirculation.Core.Fund.Books
{
    public class AuthorService : CrudService<Author>
    {
        public AuthorService(IRepository<Author> repository) : base(repository)
        {
        }
    }
}