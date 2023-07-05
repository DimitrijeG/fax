using System;
using HealthCare.Application.Common;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.PhysicalAssets
{
    public class OrderItem : RepositoryItem
    {
        public OrderItem() : this(0, 0, DateTime.MinValue, false)
        {
        }

        public OrderItem(int equipmentId, int quantity, DateTime scheduled, bool executed)
        {
            Id = 0;
            ItemId = equipmentId;
            Quantity = quantity;
            Scheduled = scheduled;
            Executed = executed;
        }

        public int Id { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime Scheduled { get; set; }
        public bool Executed { get; set; }

        public override object Key
        {
            get => Id;
            set { Id = (int)value; }
        }

        public override string[] Serialize()
        {
            return new string[]
            {
                Id.ToString(), ItemId.ToString(),
                Quantity.ToString(), Util.ToString(Scheduled),
                Executed.ToString()
            };
        }

        public override void Deserialize(string[] values)
        {
            Id = int.Parse(values[0]);
            ItemId = int.Parse(values[1]);
            Quantity = int.Parse(values[2]);
            Scheduled = Util.ParseDate(values[3]);
            Executed = bool.Parse(values[4]);
        }
    }
}