using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Scheduling.Examination
{
    public class AnamnesisService : NumericService<Anamnesis>
    {
        public AnamnesisService(IRepository<Anamnesis> repository) : base(repository)
        {
        }
    }
}