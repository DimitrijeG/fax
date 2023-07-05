using System.Collections.Generic;
using HealthCare.DataManagment.Serialize;

namespace HealthCare.DataManagment.Repository
{
    public class FileRepository<T> : IRepository<T> where T : RepositoryItem, new() // remove new()
    {
        private readonly string _filepath;
        private readonly 

        public FileRepository(string filepath)
        {
            _filepath = filepath;
        }

        public T? Get(object key)
        {
            return Load().Find(x => x.Key.Equals(key));
        }

        public virtual void Add(T item)
        {
            if (Contains(item.Key)) return;

            var items = Load();
            items.Add(item);
            Save(items);
        }

        public void Remove(object key)
        {
            var items = Load();
            items.RemoveAll(x => x.Key.Equals(key));
            Save(items);
        }

        public void Update(T item)
        {
            var items = Load();
            int i = items.FindIndex(x => x.Equals(item));
            if (i == -1) return;

            items[i] = item;
            Save(items);
        }

        public bool Contains(object key)
        {
            return Get(key) is not null;
        }

        public int Count()
        {
            return Load().Count;
        }

        public List<T> Load()
        {
            return Serializer<T>.DeserializeAll(_filepath);
        }

        private void Save(List<T> items)
        {
            Serializer<T>.SerializeAll(_filepath, items);
        }
    }
}