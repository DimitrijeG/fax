using System;
using System.Linq;
using HealthCare.Application.Common;
using HealthCare.DataManagment.Serialize;

namespace HealthCare.Core.Users.Model
{
    public class Doctor : User
    {
        public Doctor() : base()
        {
            Specialization = "";
            Random rnd = new Random();
            Rating = 0;
        }

        public Doctor(string name, string lastName, string jMBG, DateTime birthDate, string phoneNumber, string address,
            string userName, string password, Gender gender, string specialization)
            : base(name, lastName, jMBG, birthDate, phoneNumber, address, userName, password, gender)
        {
            Specialization = specialization;
            Random rnd = new Random();
            Rating = 0;
            Color = ColorRandomizer.GetRandomColor();
        }

        public Doctor(string name, string lastName, string jMBG, DateTime birthDate, string phoneNumber, string address,
            string userName, string password, Gender gender, string specialization, string color)
            : base(name, lastName, jMBG, birthDate, phoneNumber, address, userName, password, gender, color)
        {
            Specialization = specialization;
            Random rnd = new Random();
            Rating = 0;
        }

        public string Specialization { get; set; }
        public double Rating { get; set; }

        public bool IsCapable(string NeededSpecialization)
        {
            return Specialization == NeededSpecialization;
        }

        public override string[] Serialize()
        {
            string[] userValues = base.Serialize();
            return userValues.Concat(new string[] { Specialization, Rating.ToString() }).ToArray();
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
            Specialization = values[10];
            Rating = double.Parse(values[11]);
        }
    }
}