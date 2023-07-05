using System.Collections.Generic;
using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Interior
{
    public class RoomService : NumericService<Room>
    {
        public static readonly int PATIENTCARE_CAPACITY = 3;

        public RoomService(IRepository<Room> repository) : base(repository)
        {
        }

        public int GetWarehouseId()
        {
            var warehouses = GetRoomsByType(RoomType.Warehouse);
            if (warehouses.Count == 0)
                throw new KeyNotFoundException();

            return warehouses[0].Id;
        }

        public List<Room> GetRoomsByType(RoomType roomType)
        {
            return GetAll().FindAll(x => x.Type == roomType);
        }
    }
}