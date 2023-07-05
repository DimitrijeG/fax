namespace HealthCare.Core.Scheduling.Availability
{
    public interface IAvailable<T>
    {
        bool IsAvailable(T key, TimeSlot timeSlot);
    }
}