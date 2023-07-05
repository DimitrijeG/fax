using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.PatientSatisfaction
{
    public class Survey : RepositoryItem
    {
        private int? selectedRating;


        public override object Key
        {
            get => surveyID;
            set => surveyID = (int)value;
        }

        public int surveyID { get; set; }

        public string DoctorJMBG { get; set; }
        public string TopicName { get; set; }
        public string Description { get; set; }

        public int? SelectedRating
        {
            get { return selectedRating; }
            set { selectedRating = value; }
        }

        public string AdditionalComment { get; set; }

        public override void Deserialize(string[] values)
        {
            surveyID = int.Parse(values[0]);
            DoctorJMBG = values[1];
            TopicName = values[2];
            Description = values[3];
            SelectedRating = int.Parse(values[4]);
            AdditionalComment = values[5];
        }

        public override string[] Serialize()
        {
            return new string[]
            {
                surveyID.ToString(), DoctorJMBG.ToString(), TopicName, Description, SelectedRating.ToString(),
                AdditionalComment
            };
        }
    }
}