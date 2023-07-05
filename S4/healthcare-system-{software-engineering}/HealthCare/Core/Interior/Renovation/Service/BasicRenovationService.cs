using System;
using System.Collections.Generic;
using HealthCare.Core.Interior.Renovation.Model;
using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Interior.Renovation.Service
{
    public class BasicRenovationService : NumericService<RenovationBase>, IRenovationService
    {
        public BasicRenovationService(IRepository<RenovationBase> repository) : base(repository)
        {
            ExecuteAll();
        }

        public IEnumerable<RenovationBase> GetRenovations()
        {
            return GetAll();
        }

        public void Execute(RenovationBase renovation)
        {
            renovation.Executed = true;
            Update(renovation);
        }

        public void ExecuteAll()
        {
            GetAll().ForEach(x =>
            {
                if (!x.Executed && x.Scheduled.End <= DateTime.Now)
                    Execute(x);
            });
        }
    }
}