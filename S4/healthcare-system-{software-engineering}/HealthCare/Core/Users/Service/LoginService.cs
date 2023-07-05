using System;
using System.Collections.Generic;
using HealthCare.Application;
using HealthCare.Application.Exceptions;
using HealthCare.Core.Users.Model;

namespace HealthCare.Core.Users.Service
{
    public enum Role
    {
        Patient,
        Doctor,
        Nurse,
        Manager
    }

    public class LoginService
    {
        private const string ADMIN_USER = "admin";
        private const string ADMIN_PASS = "admin";
        private readonly Dictionary<Role, IUserService> _userServices;

        public LoginService()
        {
            _userServices = new Dictionary<Role, IUserService>
            {
                { Role.Patient, Injector.GetService<PatientService>() },
                { Role.Doctor, Injector.GetService<DoctorService>() },
                { Role.Nurse, Injector.GetService<NurseService>() }
            };
        }

        public Role Login(string username, string password)
        {
            if (ADMIN_USER == username)
            {
                if (ADMIN_PASS != password)
                    throw new WrongPasswordException();
                return Role.Manager;
            }

            (User user, Role role) = GetUser(username);
            if (user.Password != password)
                throw new WrongPasswordException();

            Context.Current = user;
            return role;
        }

        private (User, Role) GetUser(string username)
        {
            foreach (Role r in Enum.GetValues(typeof(Role)))
                if (!r.Equals(Role.Manager) &&
                    _userServices[r].GetAllUsers().Find(u => u.Username == username) is User u)
                    return (u, r);

            throw new UsernameNotFoundException();
        }
    }
}