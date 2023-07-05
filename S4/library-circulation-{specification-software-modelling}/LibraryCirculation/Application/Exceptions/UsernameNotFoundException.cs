using System;
using System.Runtime.Serialization;

namespace LibraryCirculation.Application.Exceptions
{
    public class UsernameNotFoundException : LoginException
    {
        public UsernameNotFoundException() : this("Nepostojeće korisnicko ime. Pokušajte ponovo.")
        {
        }

        public UsernameNotFoundException(string? message) : base(message)
        {
        }

        public UsernameNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UsernameNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}