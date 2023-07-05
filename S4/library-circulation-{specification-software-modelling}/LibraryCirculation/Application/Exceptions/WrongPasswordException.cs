using System;
using System.Runtime.Serialization;

namespace LibraryCirculation.Application.Exceptions
{
    public class WrongPasswordException : LoginException
    {
        public WrongPasswordException() : this("Pogrešna lozinka. Pokušajte ponovo.")
        {
        }

        public WrongPasswordException(string? message) : base(message)
        {
        }

        public WrongPasswordException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected WrongPasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}