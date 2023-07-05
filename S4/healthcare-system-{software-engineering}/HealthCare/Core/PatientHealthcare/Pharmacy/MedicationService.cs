using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.PatientHealthcare.Pharmacy
{
    public class MedicationService : NumericService<Medication>
    {
        public MedicationService(IRepository<Medication> repository) : base(repository)
        {
        }
    }
}