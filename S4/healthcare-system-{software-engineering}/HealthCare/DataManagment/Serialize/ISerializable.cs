namespace HealthCare.DataManagment.Serialize
{
    public interface ISerializable
    {
        string[] Serialize();

        void Deserialize(string[] values);
    }
}