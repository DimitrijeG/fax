using System.Linq;
using HealthCare.Application;
using HealthCare.Core.Interior;
using HealthCare.Core.PatientHealthcare.HealthcareTreatment;

namespace HealthCare.Core.Scheduling.Availability
{
    public class RoomTreatmentAvailable : IAvailable<int>
    {
        private readonly TreatmentService _treatmentService;

        public RoomTreatmentAvailable()
        {
            _treatmentService = Injector.GetService<TreatmentService>();
        }

        public bool IsAvailable(int key, TimeSlot timeSlot)
        {
            return _treatmentService.GetAll()
                .Where(t => t.RoomId == key)
                .Count(x => x.TreatmentDuration.Overlaps(timeSlot)) < RoomService.PATIENTCARE_CAPACITY;
        }
    }
}