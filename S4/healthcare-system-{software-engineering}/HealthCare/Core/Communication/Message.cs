using System;
using HealthCare.Application.Common;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Communication
{
    public class Message : RepositoryItem
    {
        public override object Key
        {
            get => ID;
            set => ID = (int)value;
        }

        public int ID { get; set; }

        public int contactID { get; set; }

        public string SenderJMBG { get; set; }
        public string MessageText { get; set; }
        public DateTime Time { get; set; }

        public bool Seen { get; set; }

        public override string[] Serialize()
        {
            return new string[]
                { ID.ToString(), contactID.ToString(), SenderJMBG, MessageText, Util.ToString(Time), Seen.ToString() };
        }

        public override void Deserialize(string[] values)
        {
            ID = int.Parse(values[0]);
            contactID = int.Parse(values[1]);
            SenderJMBG = values[2];
            MessageText = values[3];
            Time = Util.ParseDate(values[4]);
            Seen = Convert.ToBoolean(values[5]);
        }
    }
}