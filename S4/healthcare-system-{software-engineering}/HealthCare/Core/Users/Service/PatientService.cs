using System.Collections.Generic;
using System.Linq;
using HealthCare.Core.Service;
using HealthCare.Core.Users.Model;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Users.Service
{
    public class PatientService : Service<Patient>, IUserService
    {
        public PatientService(IRepository<Patient> repository) : base(repository)
        {
        }

        public List<User> GetAllUsers()
        {
            return GetAll().Cast<User>().ToList();
        }
    }
}