using System.Collections.Generic;
using System.Linq;
using HealthCare.DataManagment.Repository;
using HealthCare.DataManagment.Serialize;

namespace HealthCare.Core.Communication
{
    public class Contact : RepositoryItem
    {
        private int _id;

        public override object Key
        {
            get => ID;
            set => ID = (int)value;
        }

        public int ID
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                }
            }
        }

        public List<string> Participants { get; set; }

        public override string[] Serialize()
        {
            string recipients = SerialUtil.ToString(Participants);
            return new string[] { ID.ToString(), recipients };
        }

        public override void Deserialize(string[] values)
        {
            ID = int.Parse(values[0]);
            Participants = values[1].Split("|").ToList();
        }
    }
}