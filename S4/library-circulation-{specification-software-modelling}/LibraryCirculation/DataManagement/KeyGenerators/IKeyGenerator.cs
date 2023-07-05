using System.Collections.Generic;

namespace LibraryCirculation.DataManagement.KeyGenerators
{
    public interface IKeyGenerator
    {
        object Generate(object newKey, IEnumerable<object> keys);
    }
}