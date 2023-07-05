using HealthCare.DataManagment.Serialize;

namespace HealthCare.DataManagment.Repository
{
    public abstract class RepositoryItem : ISerializable
    {
        public abstract object Key { get; set; }

        public abstract string[] Serialize();
        public abstract void Deserialize(string[] values);


        public override bool Equals(object? obj)
        {
            return obj is RepositoryItem other && Key.Equals(other.Key);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}