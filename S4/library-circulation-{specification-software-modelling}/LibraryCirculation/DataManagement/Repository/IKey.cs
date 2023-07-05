namespace LibraryCirculation.DataManagement.Repository
{
    public interface IKey
    {
        object GetKey();
        void SetKey(object key);
    }
}