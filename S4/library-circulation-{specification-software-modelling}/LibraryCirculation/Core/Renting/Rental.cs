using System.Collections.Generic;
using LibraryCirculation.Core.Common;
using LibraryCirculation.DataManagement.Repository;
using LibraryCirculation.DataManagement.Serialize;

namespace LibraryCirculation.Core.Renting
{
    public class Rental : IKey, ISerializable
    {
        public Rental() : this(0, new TimeSlot(), false, 0, 0)
        {
        }

        public Rental(int id, TimeSlot time, bool returned, int copyId, int readerId)
        {
            Id = id;
            Time = time;
            Returned = returned;
            CopyId = copyId;
            ReaderId = readerId;
        }

        public int Id { get; set; }
        public TimeSlot Time { get; set; }
        public bool Returned { get; set; }
        public int CopyId { get; set; }
        public int ReaderId { get; set; }

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
                Id.ToString(), Time.Serialize(), Returned.ToString(),
                CopyId.ToString(), ReaderId.ToString()
            };
        }

        public void Deserialize(IReadOnlyList<string> values)
        {
            Id = int.Parse(values[0]);
            Time.Deserialize(values[1]);
            Returned = bool.Parse(values[2]);
            CopyId = int.Parse(values[3]);
            ReaderId = int.Parse(values[4]);
        }
    }
}