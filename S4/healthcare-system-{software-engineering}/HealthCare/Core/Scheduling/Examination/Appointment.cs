using System;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Scheduling.Examination
{
    public class Appointment : RepositoryItem
    {
        public Appointment() : this("", "", new TimeSlot(), false)
        {
        }

        public Appointment(string patientJMBG, string doctorJMBG, TimeSlot timeSlot, bool isOperation)
        {
            PatientJMBG = patientJMBG;
            DoctorJMBG = doctorJMBG;
            TimeSlot = timeSlot;
            IsOperation = isOperation;
            AnamnesisID = 0;
            RoomID = 0;
            IsUrgent = false;
        }

        public int AppointmentID { get; set; }
        public string PatientJMBG { get; set; }
        public string DoctorJMBG { get; set; }
        public int RoomID { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public bool IsOperation { get; set; }
        public int AnamnesisID { get; set; }
        public bool IsUrgent { get; set; }

        public override object Key
        {
            get => AppointmentID;
            set { AppointmentID = (int)value; }
        }

        public bool HasStarted()
        {
            return TimeSlot.Start < DateTime.Now && TimeSlot.End > DateTime.Now;
        }

        public override string[] Serialize()
        {
            return new string[]
            {
                AppointmentID.ToString(), PatientJMBG, DoctorJMBG,
                TimeSlot.ToString(), IsOperation.ToString(), AnamnesisID.ToString(), IsUrgent.ToString(),
                RoomID.ToString()
            };
        }

        public override void Deserialize(string[] values)
        {
            AppointmentID = int.Parse(values[0]);
            PatientJMBG = values[1];
            DoctorJMBG = values[2];
            TimeSlot = TimeSlot.Parse(values[3]);
            IsOperation = bool.Parse(values[4]);
            AnamnesisID = int.Parse(values[5]);
            IsUrgent = Convert.ToBoolean(values[6]);
            RoomID = int.Parse(values[7]);
        }
    }
}