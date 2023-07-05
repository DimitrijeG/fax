using System.Collections.Generic;
using System.Linq;
using LibraryCirculation.Core.Common;
using LibraryCirculation.DataManagement.Serialize;

namespace LibraryCirculation.Core.Users.Readers
{
    public class Reader : User, ISerializable
    {
        public Reader() : this("", "", "", "", "", 0, MembershipCategory.Kid, new TimeSlot(), "")
        {
        }

        public Reader(string name, string surname, string email, string username, string password, int cardId,
            MembershipCategory membership, TimeSlot membershipTime, string bankAccountNumber)
            : base(name, surname, email, username, password, Role.Reader)
        {
            CardId = cardId;
            Membership = membership;
            MembershipTime = membershipTime;
            BankAccountNumber = bankAccountNumber;
        }

        public int CardId { get; set; }
        public MembershipCategory Membership { get; set; }
        public TimeSlot MembershipTime { get; set; }
        public string BankAccountNumber { get; set; }

        public new IReadOnlyList<string> Serialize()
        {
            var serialized = new[]
            {
                CardId.ToString(),
                Membership.ToString(),
                MembershipTime.Serialize(),
                BankAccountNumber
            };
            return serialized.Concat(base.Serialize()).ToList();
        }

        public new void Deserialize(IReadOnlyList<string> values)
        {
            CardId = int.Parse(values[0]);
            Membership = SerialUtil.Parse<MembershipCategory>(values[1]);
            MembershipTime.Deserialize(values[2]);
            BankAccountNumber = values[3];
            base.Deserialize(SerialUtil.GetRange(values, 4));
        }
    }
}