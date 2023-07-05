using System.Collections.Generic;
using LibraryCirculation.Application.Common;
using LibraryCirculation.DataManagement.Repository;
using LibraryCirculation.DataManagement.Serialize;

namespace LibraryCirculation.Core.Users.Readers
{
    public enum MembershipCategory
    {
        Kid,
        Adult,
        Student,
        Senior
    }

    public class Membership : IKey, ISerializable
    {
        public Membership() : this(MembershipCategory.Kid, 0.0, 0, 0)
        {
        }

        public Membership(MembershipCategory category, double price, int maxRents, int maxRentDays)
        {
            Category = category;
            Price = price;
            MaxRents = maxRents;
            MaxRentDays = maxRentDays;
        }

        public MembershipCategory Category { get; set; }
        public double Price { get; set; }
        public int MaxRents { get; set; }
        public int MaxRentDays { get; set; }


        public object GetKey()
        {
            return Category;
        }

        public void SetKey(object key)
        {
            Category = (MembershipCategory)key;
        }

        public IReadOnlyList<string> Serialize()
        {
            return new[] { Category.ToString(), Util.ToString(Price), MaxRents.ToString(), MaxRentDays.ToString() };
        }

        public void Deserialize(IReadOnlyList<string> values)
        {
            Category = SerialUtil.Parse<MembershipCategory>(values[0]);
            Price = double.Parse(values[1]);
            MaxRents = int.Parse(values[2]);
            MaxRentDays = int.Parse(values[3]);
        }
    }
}