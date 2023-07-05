using System;
using System.Collections.Generic;
using System.Linq;
using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.PatientHealthcare.HealthcareTreatment
{
    public class TreatmentService : NumericService<Treatment>
    {
        public TreatmentService(IRepository<Treatment> repository) : base(repository)
        {
        }

        public List<Treatment> getCurrent()
        {
            return GetAll().Where(x => x.TreatmentDuration.Contains(DateTime.Now)).ToList();
        }
    }
}