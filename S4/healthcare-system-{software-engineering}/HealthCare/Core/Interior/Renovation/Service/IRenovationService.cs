using System.Collections.Generic;
using HealthCare.Core.Interior.Renovation.Model;

namespace HealthCare.Core.Interior.Renovation.Service
{
    public interface IRenovationService
    {
        public IEnumerable<RenovationBase> GetRenovations();
    }
}