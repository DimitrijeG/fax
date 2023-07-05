using System.Linq;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Service
{
    public abstract class NumericService<T> : Service<T> where T : RepositoryItem
    {
        public NumericService(IRepository<T> repository) : base(repository)
        {
        }

        public new int Add(T item)
        {
            if ((int)item.Key == 0)
                item.Key = NextId();

            base.Add(item);
            return (int)item.Key;
        }

        private int NextId()
        {
            var max = GetAll().Max(x => x.Key);
            if (max is null) return 1;
            return (int)max + 1;
        }
    }
}