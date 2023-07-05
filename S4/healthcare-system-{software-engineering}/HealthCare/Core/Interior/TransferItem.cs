using System;
using System.Linq;
using HealthCare.Application.Common;
using HealthCare.Core.PhysicalAssets;

namespace HealthCare.Core.Interior
{
    public class TransferItem : OrderItem
    {
        public TransferItem() : this(0, 0, DateTime.MinValue, false, 0, 0)
        {
        }

        public TransferItem(int equipmentId, int quantity, DateTime scheduled, bool executed, int fromRoom,
            int toRoom) :
            base(equipmentId, quantity, scheduled, executed)
        {
            FromRoom = fromRoom;
            ToRoom = toRoom;
        }

        public int FromRoom { get; set; }
        public int ToRoom { get; set; }

        public override string[] Serialize()
        {
            return base.Serialize().Concat(
                new string[] { FromRoom.ToString(), ToRoom.ToString() }).ToArray();
        }

        public override void Deserialize(string[] values)
        {
            base.Deserialize(values.SubArray(0, 5));
            FromRoom = int.Parse(values[5]);
            ToRoom = int.Parse(values[6]);
        }
    }
}