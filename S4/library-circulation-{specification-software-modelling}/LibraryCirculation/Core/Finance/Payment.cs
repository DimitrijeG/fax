using System;
using System.Collections.Generic;
using LibraryCirculation.Application.Common;
using LibraryCirculation.DataManagement.Repository;
using LibraryCirculation.DataManagement.Serialize;

namespace LibraryCirculation.Core.Finance
{
    public enum PaymentType
    {
        Compensation,
        Membership,
        Penalty
    }

    public class Payment : IKey, ISerializable
    {
        public Payment() : this(0, 0.0, PaymentType.Penalty, DateTime.MinValue)
        {
        }

        public Payment(int id, double amount, PaymentType type, DateTime date)
        {
            Id = id;
            Amount = amount;
            Type = type;
            Date = date;
        }

        public int Id { get; set; }
        public double Amount { get; set; }
        public PaymentType Type { get; set; }
        public DateTime Date { get; set; }

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
            return new[] { Id.ToString(), Util.ToString(Amount), Type.ToString(), Util.ToString(Date) };
        }

        public void Deserialize(IReadOnlyList<string> values)
        {
            Id = int.Parse(values[0]);
            Amount = double.Parse(values[1]);
            Type = SerialUtil.Parse<PaymentType>(values[2]);
            Date = Util.ParseDateTime(values[3]);
        }
    }
}