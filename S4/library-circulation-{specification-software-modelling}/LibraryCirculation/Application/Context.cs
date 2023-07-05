using LibraryCirculation.Application.Exceptions;
using LibraryCirculation.Core.Users;

namespace LibraryCirculation.Application
{
    public static class Context
    {
        private static User? _current;

        public static User Current
        {
            get => _current ?? throw new LoginException("Korisnik nije ulogovan.");
            set => _current = value;
        }

        public static void Reset()
        {
            _current = null;
        }

        public static bool IsLoggedIn()
        {
            return _current is not null;
        }
    }
}