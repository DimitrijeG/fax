using System.Collections.Generic;
using System.Linq;
using HealthCare.DataManagment.Repository;
using HealthCare.DataManagment.Serialize;

namespace HealthCare.Core.Notifications
{
    public class Notification : RepositoryItem
    {
        public Notification() : this("", new string[0])
        {
        }

        public Notification(string text, params string[] recipients) :
            this(0, text, false, recipients)
        {
        }

        public Notification(int id, string text, bool seen, params string[] recipients)
        {
            Id = id;
            Text = text;
            Seen = seen;
            Recipients = recipients.ToList();
        }

        public int Id { get; set; }
        public List<string> Recipients { get; set; }
        public string Text { get; set; }
        public bool Seen { get; set; }

        public override object Key
        {
            get => Id;
            set { Id = (int)value; }
        }

        public string Display()
        {
            Seen = true;
            return Text;
        }

        public override string[] Serialize()
        {
            string recipients = SerialUtil.ToString(Recipients);
            return new string[] { Id.ToString(), recipients, Text, Seen.ToString() };
        }

        public override void Deserialize(string[] values)
        {
            Id = int.Parse(values[0]);
            Recipients = values[1].Split("|").ToList();
            Text = values[2];
            Seen = bool.Parse(values[3]);
        }
    }
}