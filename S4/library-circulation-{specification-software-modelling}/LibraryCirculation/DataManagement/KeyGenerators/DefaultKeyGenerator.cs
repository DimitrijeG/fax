using System.Collections.Generic;

namespace LibraryCirculation.DataManagement.KeyGenerators
{
    public class DefaultKeyGenerator : IKeyGenerator
    {
        public DefaultKeyGenerator()
        {
        }

        public object Generate(object newKey, IEnumerable<object> _)
        {
            return newKey;
        }
    }
}