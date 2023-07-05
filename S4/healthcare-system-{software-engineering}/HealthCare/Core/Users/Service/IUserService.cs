using System.Collections.Generic;
using HealthCare.Core.Users.Model;

namespace HealthCare.Core.Users.Service
{
    public interface IUserService
    {
        List<User> GetAllUsers();
    }
}