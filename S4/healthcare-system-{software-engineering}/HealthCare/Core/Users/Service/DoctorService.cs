using System.Collections.Generic;
using System.Linq;
using HealthCare.Core.Service;
using HealthCare.Core.Users.Model;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Users.Service
{
    public class DoctorService : Service<Doctor>, IUserService
    {
        public DoctorService(IRepository<Doctor> repository) : base(repository)
        {
        }

        public List<User> GetAllUsers()
        {
            return GetAll().Cast<User>().ToList();
        }

        public List<string> GetBySpecialization(string specialization)
        {
            return GetAll().Where(x => x.IsCapable(specialization))
                .Select(x => x.JMBG).ToList();
        }

        public string? GetFirstBySpecialization(string specialization)
        {
            return GetBySpecialization(specialization).FirstOrDefault();
        }

        public List<string> GetSpecializations()
        {
            return GetAll().Select(x => x.Specialization).Distinct().ToList();
        }
    }
}