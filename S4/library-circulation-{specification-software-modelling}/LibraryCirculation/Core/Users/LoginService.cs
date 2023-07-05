using LibraryCirculation.Application;
using LibraryCirculation.Application.Exceptions;
using LibraryCirculation.Core.Users.Librarians;
using LibraryCirculation.Core.Users.Readers;
using LibraryCirculation.DataManagement.Repository;

namespace LibraryCirculation.Core.Users
{
    public class LoginService
    {
        private readonly IRepository<Librarian> _librarianRepository;
        private readonly IRepository<Reader> _readerRepository;

        public LoginService(IRepository<Reader> readerRepository, IRepository<Librarian> librarianRepository)
        {
            _readerRepository = readerRepository;
            _librarianRepository = librarianRepository;
        }

        public Role Login(string username, string password)
        {
            if (username == "admin" && password == "admin")
                return Role.Administrator;

            var user = GetUser(username);

            if (user.Password != password)
                throw new WrongPasswordException();

            Context.Current = user;
            return user.Role;
        }

        private User GetUser(string username)
        {
            User? user = _readerRepository.TryGet(username);
            if (user is not null) return user;

            user = _librarianRepository.TryGet(username);
            if (user is not null) return user;

            throw new UsernameNotFoundException();
        }
    }
}