using System;
using HealthCare.Application.Common;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Notifications
{
    public class UserNotification : RepositoryItem
    {
        public UserNotification() : this("", DateTime.Now, "", "", false)
        {
        }

        public UserNotification(string patientID, DateTime receiveTime, string caption, string text, bool isCustom)
        {
            this.patientID = patientID;
            this.receiveTime = receiveTime;
            this.caption = caption;
            this.text = text;
            this.isCustom = isCustom;
        }

        public override object Key
        {
            get => notificationID;
            set => notificationID = (int)value;
        }

        public int notificationID { get; set; }
        public string patientID { get; set; }

        public DateTime receiveTime { get; set; }

        public string caption { get; set; }

        public string text { get; set; }

        public bool isCustom { get; set; }

        public string[] ToCSV()
        {
            return new string[]
                { notificationID.ToString(), patientID.ToString(), Util.ToString(receiveTime), caption, text };
        }

        public void FromCSV(string[] values)
        {
            notificationID = int.Parse(values[0]);
            patientID = values[1].ToString();
            receiveTime = Util.ParseDate(values[2]);
            caption = values[3];
            text = values[4];
            isCustom = true;
        }

        public override string[] Serialize()
        {
            return new string[] { notificationID.ToString(), patientID, Util.ToString(receiveTime), caption, text };
        }

        public override void Deserialize(string[] values)
        {
            notificationID = int.Parse(values[0]);
            patientID = values[1];
            receiveTime = Util.ParseDate(values[2]);
            caption = values[3];
            text = values[4];
            isCustom = true;
        }
    }
}