using System;
using HealthCare.Application.Common;
using HealthCare.DataManagment.Repository;
using HealthCare.DataManagment.Serialize;

namespace HealthCare.Core.Users.Model
{
    public enum Gender
    {
        Male,
        Female
    }

    public class User : RepositoryItem
    {
        public User() : this("", "", "", DateTime.MinValue, "", "", "", "", Gender.Female)
        {
        }

        public User(string name, string lastName, string jMBG, DateTime birthDate, string phoneNumber, string address,
            string userName, string password, Gender gender)
        {
            Name = name;
            LastName = lastName;
            JMBG = jMBG;
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
            Address = address;
            Username = userName;
            Password = password;
            Gender = gender;
            Color = ColorRandomizer.GetRandomColor();
        }

        public User(string name, string lastName, string jMBG, DateTime birthDate, string phoneNumber, string address,
            string userName, string password, Gender gender, string color)
        {
            Name = name;
            LastName = lastName;
            JMBG = jMBG;
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
            Address = address;
            Username = userName;
            Password = password;
            Gender = gender;
            Color = color;
        }

        public string Name { get; set; }
        public string LastName { get; set; }
        public string JMBG { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; }

        public string Color { get; set; }

        public override object Key
        {
            get => JMBG;
            set { JMBG = (string)value; }
        }

        public override string[] Serialize()
        {
            return new string[]
            {
                Name, LastName, JMBG, Util.ToString(BirthDate),
                PhoneNumber, Address, Username, Password, Gender.ToString(), Color
            };
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
        }
    }
}