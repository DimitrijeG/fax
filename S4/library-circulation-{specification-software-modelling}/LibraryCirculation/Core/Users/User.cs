using System.Collections.Generic;
using LibraryCirculation.DataManagement.Repository;
using LibraryCirculation.DataManagement.Serialize;

namespace LibraryCirculation.Core.Users
{
    public enum Role
    {
        Reader,
        Librarian,
        ChiefLibrarian,
        Administrator
    }

    public class User : IKey, ISerializable
    {
        public User() : this("", "", "", "", "", Role.Reader)
        {
        }

        public User(string name, string surname, string email, string username, string password, Role role)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Username = username;
            Password = password;
            Role = role;
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public object GetKey()
        {
            return Username;
        }

        public void SetKey(object key)
        {
            Username = (string)key;
        }

        public IReadOnlyList<string> Serialize()
        {
            return new[] { Name, Surname, Email, Username, Password, Role.ToString() };
        }

        public void Deserialize(IReadOnlyList<string> values)
        {
            Name = values[0];
            Surname = values[1];
            Email = values[2];
            Username = values[3];
            Password = values[4];
            Role = SerialUtil.Parse<Role>(values[5]);
        }
    }
}