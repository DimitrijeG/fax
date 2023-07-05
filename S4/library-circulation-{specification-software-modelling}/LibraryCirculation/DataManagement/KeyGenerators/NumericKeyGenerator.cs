using System.Collections.Generic;
using System.Linq;

namespace LibraryCirculation.DataManagement.KeyGenerators
{
    class NumericKeyGenerator : IKeyGenerator
    {
        public NumericKeyGenerator()
        {
        }

        public object Generate(object newKey, IEnumerable<object> keys)
        {
            if ((int)newKey != 0) return newKey;

            var i = 1;
            var intKeys = keys.Cast<int>().ToList();
            while (intKeys.Contains(i)) ++i;
            return i;
        }
    }
}