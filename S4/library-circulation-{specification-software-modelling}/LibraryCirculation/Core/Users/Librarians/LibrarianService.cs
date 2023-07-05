using LibraryCirculation.Core.Common;
using LibraryCirculation.DataManagement.Repository;

namespace LibraryCirculation.Core.Users.Librarians
{
    public class LibrarianService : CrudService<Librarian>
    {
        public LibrarianService(IRepository<Librarian> repository) : base(repository)
        {
        }
    }
}