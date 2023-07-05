using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.PatientHealthcare.Pharmacy
{
    public class TherapyService : NumericService<Therapy>
    {
        public TherapyService(IRepository<Therapy> repository) : base(repository)
        {
        }
    }
}