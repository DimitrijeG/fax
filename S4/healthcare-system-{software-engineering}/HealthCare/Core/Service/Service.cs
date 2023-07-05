using System.Collections.Generic;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Service
{
    public abstract class Service<T> where T : RepositoryItem
    {
        protected readonly IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        // can return null if not found
        public T? TryGet(object key)
        {
            return _repository.Get(key);
        }

        // throws an error if object not found
        public T Get(object key)
        {
            T? item = _repository.Get(key);
            if (item is null) throw new KeyNotFoundException();
            return item;
        }

        public void Add(T item)
        {
            _repository.Add(item);
        }

        public void Remove(object key)
        {
            _repository.Remove(key);
        }

        public void Update(T item)
        {
            _repository.Update(item);
        }

        public bool Contains(object key)
        {
            return _repository.Contains(key);
        }

        public List<T> GetAll()
        {
            return _repository.Load();
        }
    }
}