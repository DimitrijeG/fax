using System.Collections.Generic;
using System.Linq;
using HealthCare.DataManagment.Repository;
using HealthCare.DataManagment.Serialize;

namespace HealthCare.Core.PatientHealthcare.HealthcareTreatment
{
    public class TreatmentReferral : RepositoryItem
    {
        public TreatmentReferral()
        {
        }

        public TreatmentReferral(int daysOfTreatment, string patientJMBG, string doctorJMBG, int therapyId,
            List<string> additionalExamination)
        {
            DaysOfTreatment = daysOfTreatment;
            PatientJMBG = patientJMBG;
            DoctorJMBG = doctorJMBG;
            TherapyID = therapyId;
            AdditionalExamination = additionalExamination;
            IsUsed = false;
        }

        public override object Key
        {
            get => Id;
            set { Id = (int)value; }
        }

        public int Id { get; set; }
        public int DaysOfTreatment { get; set; }
        public string PatientJMBG { get; set; }
        public string DoctorJMBG { get; set; }
        public int TherapyID { get; set; }
        public List<string> AdditionalExamination { get; set; }
        public bool IsUsed { get; set; }

        public override void Deserialize(string[] values)
        {
            Id = int.Parse(values[0]);
            DaysOfTreatment = int.Parse(values[1]);
            PatientJMBG = values[2];
            DoctorJMBG = values[3];
            IsUsed = bool.Parse(values[4]);
            TherapyID = int.Parse(values[5]);
            AdditionalExamination = values[6].Split("|").ToList();
        }

        public override string[] Serialize()
        {
            return new string[]
            {
                Id.ToString(), DaysOfTreatment.ToString(), PatientJMBG, DoctorJMBG, IsUsed.ToString(),
                TherapyID.ToString(), SerialUtil.ToString(AdditionalExamination)
            };
        }
    }
}