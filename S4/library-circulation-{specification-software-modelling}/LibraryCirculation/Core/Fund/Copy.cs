using System.Collections.Generic;
using LibraryCirculation.Application.Common;
using LibraryCirculation.DataManagement.Repository;
using LibraryCirculation.DataManagement.Serialize;

namespace LibraryCirculation.Core.Fund
{
    public class Copy : IKey, ISerializable
    {
        public Copy() : this(0, 0.0, "", 0)
        {
        }

        public Copy(int id, double price, string bookIsbn, int inventoryBookId)
        {
            Id = id;
            Price = price;
            BookIsbn = bookIsbn;
            InventoryBookId = inventoryBookId;
        }

        public int Id { get; set; }
        public double Price { get; set; }
        public string BookIsbn { get; set; }
        public int InventoryNumber { get; set; }
        public int InventoryBookId { get; set; }


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
            return new[]
            {
                Id.ToString(), Util.ToString(Price), BookIsbn, InventoryNumber.ToString(), InventoryBookId.ToString()
            };
        }

        public void Deserialize(IReadOnlyList<string> values)
        {
            Id = int.Parse(values[0]);
            Price = double.Parse(values[1]);
            BookIsbn = values[2];
            InventoryNumber = int.Parse(values[3]);
            InventoryBookId = int.Parse(values[4]);
        }
    }
}