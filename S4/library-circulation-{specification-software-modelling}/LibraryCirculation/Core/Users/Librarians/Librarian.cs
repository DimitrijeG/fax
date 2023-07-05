using System.Collections.Generic;
using System.Linq;
using LibraryCirculation.Application.Common;
using LibraryCirculation.DataManagement.Serialize;

namespace LibraryCirculation.Core.Users.Librarians
{
    public class Librarian : User, ISerializable
    {
        public Librarian()
        {
        }

        public Librarian(string name, string surname, string email, string username, string password, int branchId,
            double salary, bool isChief)
            : base(name, surname, email, username, password, isChief ? Role.ChiefLibrarian : Role.Librarian)
        {
            BranchId = branchId;
            Salary = salary;
            IsChief = isChief;
        }

        public int BranchId { get; set; }
        public double Salary { get; set; }
        public bool IsChief { get; set; }

        public new IReadOnlyList<string> Serialize()
        {
            var serialized = new[]
            {
                BranchId.ToString(),
                Util.ToString(Salary),
                IsChief.ToString()
            };
            return serialized.Concat(base.Serialize()).ToList();
        }

        public new void Deserialize(IReadOnlyList<string> values)
        {
            BranchId = int.Parse(values[0]);
            Salary = double.Parse(values[1]);
            IsChief = bool.Parse(values[2]);
            base.Deserialize(SerialUtil.GetRange(values, 3));
        }
    }
}