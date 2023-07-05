using System;
using System.Collections.Generic;
using System.Linq;
using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.PatientHealthcare.HealthcareTreatment
{
    public class VisitService : NumericService<Visit>
    {
        public VisitService(IRepository<Visit> repository) : base(repository)
        {
        }

        public List<Visit> getVisits()
        {
            bool isMorning = DateTime.Now < DateTime.Now.Date.AddHours(12);
            return GetAll().Where(x => x.isMorningVisit() == isMorning && x.isToday()).ToList();
        }

        public List<Visit> GetVisitsForTreatment(int treatmentID)
        {
            return GetAll().Where(x => x.TreatmentId == treatmentID).ToList();
        }
    }
}