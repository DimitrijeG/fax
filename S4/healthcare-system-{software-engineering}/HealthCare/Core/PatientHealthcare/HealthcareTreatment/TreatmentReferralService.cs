using System.Collections.Generic;
using System.Linq;
using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.PatientHealthcare.HealthcareTreatment
{
    public class TreatmentReferralService : NumericService<TreatmentReferral>
    {
        public TreatmentReferralService(IRepository<TreatmentReferral> repository) : base(repository)
        {
        }

        public List<TreatmentReferral> GetPatientsReferrals(string patientJMBG)
        {
            return GetAll()
                .Where(x => x.PatientJMBG == patientJMBG && !x.IsUsed)
                .ToList();
        }
    }
}