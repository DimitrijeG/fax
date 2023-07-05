using LibraryCirculation.Core.Common;
using LibraryCirculation.DataManagement.Repository;

namespace LibraryCirculation.Core.Fund.Books
{
    public class BookService : CrudService<Book>
    {
        public BookService(IRepository<Book> repository) : base(repository)
        {
        }
    }
}