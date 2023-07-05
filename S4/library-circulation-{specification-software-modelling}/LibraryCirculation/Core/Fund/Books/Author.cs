using System.Collections.Generic;
using LibraryCirculation.DataManagement.Repository;
using LibraryCirculation.DataManagement.Serialize;

namespace LibraryCirculation.Core.Fund.Books
{
    public class Author : IKey, ISerializable
    {
        public Author() : this(0, "", "")
        {
        }

        public Author(int id, string displayName, string country)
        {
            Id = id;
            DisplayName = displayName;
            Country = country;
        }

        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Country { get; set; }

        public object GetKey()
        {
            return Id;
        }

        public void SetKey(object key)
        {
            Id = (int)key;
        }

        public IReadOnlyList<string> Serialize()
        {
            return new[] { Id.ToString(), DisplayName, Country };
        }

        public void Deserialize(IReadOnlyList<string> values)
        {
            Id = int.Parse(values[0]);
            DisplayName = values[1];
            Country = values[2];
        }
    }
}