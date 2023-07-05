using System.Linq;
using HealthCare.Application.Common;
using HealthCare.Core.Scheduling;

namespace HealthCare.Core.Interior.Renovation.Model
{
    public class JoiningRenovation : RenovationBase
    {
        public JoiningRenovation() : this(0, new TimeSlot(), 0, new Room())
        {
        }

        public JoiningRenovation(int roomId, TimeSlot scheduled, int otherRoomId, Room resultRoom)
            : base(roomId, scheduled)
        {
            OtherRoomId = otherRoomId;
            ResultRoom = resultRoom;
        }

        public int OtherRoomId { get; set; }
        public Room ResultRoom { get; set; }

        public override void Deserialize(string[] values)
        {
            base.Deserialize(values.SubArray(0, 4));
            OtherRoomId = int.Parse(values[4]);
            ResultRoom.Deserialize(values[5].Split('|'));
        }

        public override string[] Serialize()
        {
            string room = string.Join('|', ResultRoom.Serialize());
            return base.Serialize().Concat(new string[] { OtherRoomId.ToString(), room }).ToArray();
        }
    }
}