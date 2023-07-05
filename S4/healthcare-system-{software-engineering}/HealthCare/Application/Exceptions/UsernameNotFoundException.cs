using System;
using System.Runtime.Serialization;

namespace HealthCare.Application.Exceptions
{
    class UsernameNotFoundException : LoginException
    {
        public UsernameNotFoundException() : this("Nepostojece korisnicko ime. Pokusajte ponovo")
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