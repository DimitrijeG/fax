using System.Collections.Generic;
using System.Linq;
using LibraryCirculation.DataManagement.KeyGenerators;
using LibraryCirculation.DataManagement.Serialize;

namespace LibraryCirculation.DataManagement.Repository
{
    public class FileRepository<T> : IRepository<T> where T : IKey, ISerializable, new()
    {
        private readonly string _filepath;
        private readonly IKeyGenerator _keyGenerator;

        public FileRepository(string filepath, IKeyGenerator keyGenerator)
        {
            _filepath = filepath;
            _keyGenerator = keyGenerator;
        }

        // can return null
        public T? TryGet(object key)
        {
            return Load().Find(x => x.GetKey().Equals(key));
        }

        // must return a value
        public T Get(object key)
        {
            var item = TryGet(key);
            return item is not null ? item : throw new KeyNotFoundException();
        }

        public void Add(T item)
        {
            if (Contains(item.GetKey()))
                return;

            var newKey = _keyGenerator.Generate(item.GetKey(), GetKeys());
            item.SetKey(newKey);

            var items = Load();
            items.Add(item);
            Save(items);
        }

        public void Remove(object key)
        {
            var items = Load();
            items.RemoveAll(x => x.GetKey().Equals(key));
            Save(items);
        }

        public void Update(T item)
        {
            var items = Load();
            var i = items.FindIndex(x => x.GetKey().Equals(item.GetKey()));
            if (i == -1) return;

            items[i] = item;
            Save(items);
        }

        public bool Contains(object key)
        {
            return TryGet(key) is not null;
        }

        public int Count()
        {
            return Load().Count;
        }

        public List<T> Load()
        {
            return Serializer<T>.DeserializeAll(_filepath);
        }

        private IEnumerable<object> GetKeys()
        {
            return Load().Select(x => x.GetKey());
        }

        private void Save(List<T> items)
        {
            Serializer<T>.SerializeAll(_filepath, items);
        }
    }
}