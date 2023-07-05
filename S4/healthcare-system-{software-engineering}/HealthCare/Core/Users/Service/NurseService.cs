using System.Collections.Generic;
using System.Linq;
using HealthCare.Core.Service;
using HealthCare.Core.Users.Model;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Users.Service
{
    public class NurseService : Service<User>, IUserService
    {
        public NurseService(IRepository<User> repository) : base(repository)
        {
        }

        public List<User> GetAllUsers()
        {
            return GetAll().Cast<User>().ToList();
        }
    }
}