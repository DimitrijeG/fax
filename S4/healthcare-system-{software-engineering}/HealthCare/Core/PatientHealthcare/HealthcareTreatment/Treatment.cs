using HealthCare.Core.Scheduling;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.PatientHealthcare.HealthcareTreatment
{
    public class Treatment : RepositoryItem
    {
        public Treatment()
        {
        }

        public Treatment(int roomId, int referralId, TimeSlot treatmentDuration)
        {
            RoomId = roomId;
            ReferralId = referralId;
            TreatmentDuration = treatmentDuration;
        }

        public override object Key
        {
            get => Id;
            set => Id = (int)value;
        }

        public int Id { get; set; }
        public int RoomId { get; set; }
        public int ReferralId { get; set; }
        public TimeSlot TreatmentDuration { get; set; }


        public override string[] Serialize()
        {
            return new[] { Id.ToString(), RoomId.ToString(), ReferralId.ToString(), TreatmentDuration.ToString() };
        }

        public override void Deserialize(string[] values)
        {
            Id = int.Parse(values[0]);
            RoomId = int.Parse(values[1]);
            ReferralId = int.Parse(values[2]);
            TreatmentDuration = TimeSlot.Parse(values[3]);
        }
    }
}