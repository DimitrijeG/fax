using System.Collections.Generic;
using LibraryCirculation.DataManagement.Repository;
using LibraryCirculation.DataManagement.Serialize;

namespace LibraryCirculation.Core.Fund.Organization
{
    public class InventoryBook : IKey, ISerializable
    {
        public InventoryBook() : this(0, "", 0)
        {
        }

        public InventoryBook(int id, string title, int branchId)
        {
            Id = id;
            Title = title;
            BranchId = branchId;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int BranchId { get; set; }

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
            return new[] { Id.ToString(), Title, BranchId.ToString() };
        }

        public void Deserialize(IReadOnlyList<string> values)
        {
            Id = int.Parse(values[0]);
            Title = values[1];
            BranchId = int.Parse(values[2]);
        }
    }
}