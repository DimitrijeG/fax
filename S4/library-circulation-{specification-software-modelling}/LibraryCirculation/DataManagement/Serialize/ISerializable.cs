using System.Collections.Generic;

namespace LibraryCirculation.DataManagement.Serialize
{
    public interface ISerializable
    {
        IReadOnlyList<string> Serialize();

        void Deserialize(IReadOnlyList<string> values);
    }
}