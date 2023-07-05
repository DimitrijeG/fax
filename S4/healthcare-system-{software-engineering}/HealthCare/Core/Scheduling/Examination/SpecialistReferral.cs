using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Scheduling.Examination
{
    public class SpecialistReferral : RepositoryItem
    {
        public SpecialistReferral()
        {
        }

        public SpecialistReferral(string patientJMBG, string doctorJMBG, string referredDoctorJMBG)
        {
            PatientJMBG = patientJMBG;
            DoctorJMBG = doctorJMBG;
            ReferredDoctorJMBG = referredDoctorJMBG;
            IsUsed = false;
        }

        public override object Key
        {
            get => Id;
            set { Id = (int)value; }
        }

        public int Id { get; set; }
        public string PatientJMBG { get; set; }
        public string DoctorJMBG { get; set; }
        public string ReferredDoctorJMBG { get; set; }
        public bool IsUsed { get; set; }

        public override void Deserialize(string[] values)
        {
            Id = int.Parse(values[0]);
            PatientJMBG = values[1];
            DoctorJMBG = values[2];
            ReferredDoctorJMBG = values[3];
            IsUsed = bool.Parse(values[4]);
        }

        public override string[] Serialize()
        {
            return new string[] { Id.ToString(), PatientJMBG, DoctorJMBG, ReferredDoctorJMBG, IsUsed.ToString() };
        }
    }
}