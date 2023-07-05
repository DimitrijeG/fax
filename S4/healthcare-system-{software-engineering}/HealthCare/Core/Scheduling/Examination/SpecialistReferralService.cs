using System.Collections.Generic;
using System.Linq;
using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Scheduling.Examination
{
    public class SpecialistReferralService : NumericService<SpecialistReferral>
    {
        public SpecialistReferralService(IRepository<SpecialistReferral> repository) : base(repository)
        {
        }

        public List<SpecialistReferral> GetPatientsReferrals(string patientJMBG)
        {
            return GetAll()
                .Where(x => x.PatientJMBG == patientJMBG && !x.IsUsed)
                .ToList();
        }
    }
}