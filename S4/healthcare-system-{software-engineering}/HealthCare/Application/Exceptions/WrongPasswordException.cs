using System;
using System.Runtime.Serialization;

namespace HealthCare.Application.Exceptions
{
    public class WrongPasswordException : LoginException
    {
        public WrongPasswordException() : this("Pogresna lozinka. Pokusajte ponovo.")
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