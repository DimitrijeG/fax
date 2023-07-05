using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Core.PatientHealthcare.HealthcareTreatment;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Treatments.Visiting
{
    public class VisitDTO : ViewModelBase
    {
        private readonly Visit _visit;
        public int Id => _visit.Id;
        public double Temperature => _visit.Temperature;
        public int SystolicPressure => _visit.SystolicPressure;
        public int DiastolicPressure => _visit.DiastolicPressure;
        public string Observations => _visit.Observations;
        public DateTime VisitTime => _visit.VisitTime;

        public VisitDTO(Visit visit)
        {
            _visit = visit;
        }

    }
}
