using System;
using System.Collections.Generic;
using HealthCare.Application.Common;
using HealthCare.DataManagment.Repository;
using HealthCare.DataManagment.Serialize;

namespace HealthCare.Core.PatientHealthcare.Pharmacy
{
    public enum MealTime
    {
        BeforeMeal,
        AfterMeal,
        DuringMeal,
        NoPreference
    }

    public class Prescription : RepositoryItem
    {
        public Prescription()
        {
        }

        public Prescription(int medicationId, MealTime instruction, string patientJMBG, string doctorJMBG,
            int dailyDosage, int hoursBetweenConsumption, int consumptionDays)
        {
            MedicationId = medicationId;
            Instruction = instruction;
            PatientJMBG = patientJMBG;
            DoctorJMBG = doctorJMBG;
            DailyDosage = dailyDosage;
            HoursBetweenConsumption = hoursBetweenConsumption;
            ConsumptionDays = consumptionDays;
            Start = DateTime.Now;
            FirstUse = false;
        }

        public override object Key
        {
            get => Id;
            set { Id = (int)value; }
        }

        public int Id { get; set; }
        public int MedicationId { get; set; }
        public int DailyDosage { get; set; }
        public int HoursBetweenConsumption { get; set; }
        public int ConsumptionDays { get; set; }
        public MealTime Instruction { get; set; }
        public string PatientJMBG { get; set; }
        public string DoctorJMBG { get; set; }
        public DateTime Start { get; set; }
        public bool FirstUse { get; set; }

        public int GetQuantity()
        {
            return DailyDosage * ConsumptionDays;
        }

        public override void Deserialize(string[] values)
        {
            Id = int.Parse(values[0]);
            MedicationId = int.Parse(values[1]);
            Instruction = SerialUtil.ParseEnum<MealTime>(values[2]);
            PatientJMBG = values[3];
            DoctorJMBG = values[4];
            DailyDosage = int.Parse(values[5]);
            HoursBetweenConsumption = int.Parse(values[6]);
            ConsumptionDays = int.Parse(values[7]);
            Start = Util.ParseDate(values[8]);
            FirstUse = bool.Parse(values[9]);
        }

        public override string[] Serialize()
        {
            return new string[]
            {
                Id.ToString(), MedicationId.ToString(), Instruction.ToString(), PatientJMBG, DoctorJMBG,
                DailyDosage.ToString(), HoursBetweenConsumption.ToString(), ConsumptionDays.ToString(),
                Util.ToString(Start), FirstUse.ToString()
            };
        }

        public List<DateTime> GetPillConsumptionTimes()
        {
            List<DateTime> consumptionTimes = new List<DateTime>();
            DateTime startDate = Start.Date;
            int daysCounter = 0;

            while (daysCounter < ConsumptionDays)
            {
                int pillsCounter = 0;
                startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 8, 0, 0);
                startDate = startDate.AddDays(1);

                while (pillsCounter < DailyDosage)
                {
                    consumptionTimes.Add(startDate);
                    startDate = startDate.AddHours(HoursBetweenConsumption);
                    pillsCounter++;
                }

                daysCounter++;
            }

            return consumptionTimes;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Id.ToString(), MedicationId.ToString(), Instruction.ToString(), PatientJMBG, DoctorJMBG,
                DailyDosage.ToString(), HoursBetweenConsumption.ToString(), ConsumptionDays.ToString(),
                Util.ToString(Start), FirstUse.ToString()
            };
        }
    }
}