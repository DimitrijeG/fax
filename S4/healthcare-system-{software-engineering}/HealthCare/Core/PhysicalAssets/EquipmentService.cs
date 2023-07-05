using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.PhysicalAssets
{
    public class EquipmentService : NumericService<Equipment>
    {
        public EquipmentService(IRepository<Equipment> repository) : base(repository)
        {
        }
    }
}