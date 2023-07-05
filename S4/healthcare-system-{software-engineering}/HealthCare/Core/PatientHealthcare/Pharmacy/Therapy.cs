using System.Collections.Generic;
using HealthCare.DataManagment.Repository;
using HealthCare.WPF.Common;

namespace HealthCare.Core.PatientHealthcare.Pharmacy
{
    public class Therapy : RepositoryItem
    {
        public Therapy() : this(new(), "")
        {
        }

        public Therapy(List<int> initialMedication, string patientJMBG)
        {
            InitialMedication = initialMedication;
            PatientJMBG = patientJMBG;
        }

        public override object Key
        {
            get => Id;
            set => Id = (int)value;
        }

        public int Id { get; set; }
        public List<int> InitialMedication { get; set; }
        public string PatientJMBG { get; set; }

        public override void Deserialize(string[] values)
        {
            Id = int.Parse(values[0]);
            PatientJMBG = values[1];
            if (!string.IsNullOrWhiteSpace(values[2]))
                InitialMedication = ViewUtil.GetIntList(values[2], '|');
        }

        public override string[] Serialize()
        {
            return new string[] { Id.ToString(), PatientJMBG, string.Join('|', InitialMedication) };
        }
    }
}