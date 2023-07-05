using HealthCare.Core.Scheduling;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.HumanResources
{
    public class AbsenceRequest : RepositoryItem
    {
        public AbsenceRequest()
        {
        }

        public AbsenceRequest(string requesterJmbg, string reason, TimeSlot absenceDuration)
        {
            RequesterJMBG = requesterJmbg;
            Reason = reason;
            AbsenceDuration = absenceDuration;
            IsApproved = false;
        }

        public override object Key
        {
            get => Id;
            set => Id = (int)value;
        }

        public int Id { get; set; }
        public string RequesterJMBG { get; set; }
        public string Reason { get; set; }
        public TimeSlot AbsenceDuration { get; set; }
        public bool IsApproved { get; set; }

        public override string[] Serialize()
        {
            return new[] { Id.ToString(), RequesterJMBG, Reason, IsApproved.ToString(), AbsenceDuration.ToString() };
        }

        public override void Deserialize(string[] values)
        {
            Id = int.Parse(values[0]);
            RequesterJMBG = values[1];
            Reason = values[2];
            IsApproved = bool.Parse(values[3]);
            AbsenceDuration = TimeSlot.Parse(values[4]);
        }
    }
}