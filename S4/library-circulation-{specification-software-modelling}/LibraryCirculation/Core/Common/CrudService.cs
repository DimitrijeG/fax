using System.Collections.Generic;
using System.Linq;
using LibraryCirculation.DataManagement.Repository;
using LibraryCirculation.DataManagement.Serialize;

namespace LibraryCirculation.Core.Common
{
    public class CrudService<T> where T : IKey, ISerializable, new()
    {
        protected readonly IRepository<T> Repository;

        public CrudService(IRepository<T> repository)
        {
            Repository = repository;
        }

        public T Get(object key)
        {
            return Repository.Get(key);
        }

        public T? TryGet(object key)
        {
            return Repository.TryGet(key);
        }

        public virtual void Add(T item)
        {
            Repository.Add(item);
        }

        public void Remove(object key)
        {
            Repository.Remove(key);
        }

        public virtual void Update(T item)
        {
            Repository.Update(item);
        }

        public bool Contains(object key)
        {
            return Repository.Contains(key);
        }

        public List<T> GetAll()
        {
            return Repository.Load();
        }

        public void RemoveAll()
        {
            GetAll().Select(x => x.GetKey()).ToList().ForEach(Remove);
        }
    }
}