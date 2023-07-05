using System;
using LibraryCirculation.Application.Common;

namespace LibraryCirculation.Core.Common
{
    public class TimeSlot
    {
        public TimeSlot() : this(DateTime.MinValue, TimeSpan.Zero)
        {
        }

        public TimeSlot(TimeSlot other) : this(other.Start, other.Duration)
        {
        }

        public TimeSlot(DateTime start, DateTime end) : this(start, end - start)
        {
        }

        public TimeSlot(DateTime start, TimeSpan duration)
        {
            Start = start;
            Duration = duration;
        }

        public DateTime Start { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime End => Start + Duration;

        public bool Overlaps(DateTime start, DateTime end)
        {
            return start < End && end > Start;
        }

        public bool Contains(DateTime date)
        {
            return date >= Start && date <= End;
        }

        public string Serialize()
        {
            return Util.ToString(Start) + "/" + Util.ToString(Duration);
        }

        public void Deserialize(string str)
        {
            var values = str.Split("/");
            Start = Util.ParseDateTime(values[0]);
            Duration = Util.ParseTimeSpan(values[1]);
        }
    }
}