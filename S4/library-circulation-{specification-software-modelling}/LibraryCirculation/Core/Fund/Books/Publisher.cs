using System.Collections.Generic;
using LibraryCirculation.DataManagement.Repository;
using LibraryCirculation.DataManagement.Serialize;

namespace LibraryCirculation.Core.Fund.Books
{
    public class Publisher : IKey, ISerializable
    {
        public Publisher() : this(0, "", "")
        {
        }

        public Publisher(int id, string displayName, string city)
        {
            Id = id;
            DisplayName = displayName;
            City = city;
        }

        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string City { get; set; }

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
            return new[] { Id.ToString(), DisplayName, City };
        }

        public void Deserialize(IReadOnlyList<string> values)
        {
            Id = int.Parse(values[0]);
            DisplayName = values[1];
            City = values[2];
        }
    }
}