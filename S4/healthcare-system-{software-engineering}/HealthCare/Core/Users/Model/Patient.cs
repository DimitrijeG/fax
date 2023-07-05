using System;
using System.Collections.Generic;
using System.Linq;
using HealthCare.Application.Common;
using HealthCare.Core.PatientHealthcare;
using HealthCare.DataManagment.Serialize;

namespace HealthCare.Core.Users.Model
{
    public class Patient : User
    {
        public Patient(string name, string lastName, string jMBG, DateTime birthDate, string phoneNumber,
            string address, string userName, string password, Gender gender, bool blocked, MedicalRecord? medicalRecord)
            : base(name, lastName, jMBG, birthDate, phoneNumber, address, userName, password, gender)
        {
            Blocked = blocked;
            MedicalRecord = medicalRecord;
            NotificationHours = 0;
            Color = ColorRandomizer.GetRandomColor();
        }

        public Patient(string name, string lastName, string jMBG, DateTime birthDate, string phoneNumber,
            string address, string userName, string password, Gender gender, bool blocked, MedicalRecord? medicalRecord,
            int notificationHours)
            : base(name, lastName, jMBG, birthDate, phoneNumber, address, userName, password, gender)
        {
            Blocked = blocked;
            MedicalRecord = medicalRecord;
            NotificationHours = notificationHours;
            Color = ColorRandomizer.GetRandomColor();
        }

        public Patient()
        {
        }

        public bool Blocked { get; set; }
        public MedicalRecord? MedicalRecord { get; set; }

        public int NotificationHours { get; set; }

        public bool IsAllergic(IEnumerable<string> ingredients)
        {
            return ingredients.Any(x => MedicalRecord.Allergies.Contains(x));
        }

        public override string[] Serialize()
        {
            string[] userValues = base.Serialize();
            string[] patientValues = { Blocked.ToString() };
            string[] patientNotificationHours = { NotificationHours.ToString() };
            string[] medicalRecordValues = { "" };
            int patientNotifications = NotificationHours;
            if (MedicalRecord != null)
                medicalRecordValues = MedicalRecord.Serialize();
            string[] csvValues = userValues.Concat(patientValues).Concat(medicalRecordValues)
                .Concat(patientNotificationHours).ToArray();
            return csvValues;
        }

        public override void Deserialize(string[] values)
        {
            Name = values[0];
            LastName = values[1];
            JMBG = values[2];
            BirthDate = Util.ParseDate(values[3]);
            PhoneNumber = values[4];
            Address = values[5];
            Username = values[6];
            Password = values[7];
            Gender = SerialUtil.ParseEnum<Gender>(values[8]);
            Color = values[9];
            Blocked = bool.Parse(values[10]);
            MedicalRecord = new MedicalRecord();
            MedicalRecord.Deserialize(values.SubArray(11, 4));
            NotificationHours = int.Parse(values[15]);
        }
    }
}