using HealthCare.DataManagment.Repository;
using HealthCare.DataManagment.Serialize;

namespace HealthCare.Core.PhysicalAssets
{
    public enum EquipmentType
    {
        Examinational,
        Operational,
        RoomFurniture,
        HallwayFurniture,
        Medication
    }

    public class Equipment : RepositoryItem
    {
        public Equipment() : this(0, "", EquipmentType.Examinational, false)
        {
        }

        public Equipment(int id, string name, EquipmentType type, bool dynamic)
        {
            Id = id;
            Name = name;
            Type = type;
            IsDynamic = dynamic;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public EquipmentType Type { get; set; }
        public bool IsDynamic { get; set; }

        public override object Key
        {
            get => Id;
            set { Id = (int)value; }
        }

        public override string[] Serialize()
        {
            return new string[] { Id.ToString(), Name, Type.ToString(), IsDynamic.ToString() };
        }

        public override void Deserialize(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            Type = SerialUtil.ParseEnum<EquipmentType>(values[2]);
            IsDynamic = bool.Parse(values[3]);
        }
    }
}