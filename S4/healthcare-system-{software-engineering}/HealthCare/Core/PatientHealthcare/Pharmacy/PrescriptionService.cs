using System.Collections.Generic;
using System.Linq;
using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.PatientHealthcare.Pharmacy
{
    public class PrescriptionService : NumericService<Prescription>
    {
        public PrescriptionService(IRepository<Prescription> repository) : base(repository)
        {
        }

        public List<Prescription> GetPatientsPrescriptions(string JMBG)
        {
            return GetAll().Where(x => x.PatientJMBG == JMBG).ToList();
        }
    }
}