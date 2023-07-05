using System.Collections.Generic;
using System.Linq;
using HealthCare.Application;
using HealthCare.Core.Interior;
using HealthCare.Core.Scheduling.Availability;
using HealthCare.Core.Scheduling.Examination;

namespace HealthCare.Core.Scheduling.Schedules
{
    public class RoomSchedule : ScheduleBase<int>, IAppointmentAvailable
    {
        private readonly RoomService _roomService;

        public RoomSchedule()
        {
            _roomService = Injector.GetService<RoomService>();
            _availabilityValidators = new List<IAvailable<int>>
            {
                new RoomRenovationAvailable(),
                new RoomAppointmentAvailable(),
                new RoomTreatmentAvailable()
            };
        }

        public bool IsAvailable(Appointment appointment)
        {
            return appointment.RoomID == 0 ||
                   IsAvailable(appointment.RoomID, appointment.TimeSlot);
        }

        public void SetFirstAvailableRoom(Appointment appointment)
        {
            RoomType type = appointment.IsOperation
                ? RoomType.Operational
                : RoomType.Examinational;

            appointment.RoomID = GetAvailableRoomsByType(type, appointment.TimeSlot)
                .FirstOrDefault(0);
        }

        public List<int> GetAvailableRoomsByType(RoomType type, TimeSlot timeSlot)
        {
            return _roomService
                .GetRoomsByType(type)
                .Where(r => IsAvailable(r.Id, timeSlot))
                .Select(r => r.Id)
                .ToList();
        }
    }
}