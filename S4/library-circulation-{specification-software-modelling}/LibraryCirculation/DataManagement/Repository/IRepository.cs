using System.Collections.Generic;

namespace LibraryCirculation.DataManagement.Repository
{
    public interface IRepository<T> where T : IKey
    {
        T? TryGet(object key);
        T Get(object key);
        void Add(T item);
        void Remove(object key);
        void Update(T item);
        bool Contains(object key);
        int Count();
        List<T> Load();
    }
}