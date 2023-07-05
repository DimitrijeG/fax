using System.Collections.Generic;
using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Notifications
{
    public class UserNotificationService : NumericService<UserNotification>
    {
        public UserNotificationService(IRepository<UserNotification> repository) : base(repository)
        {
        }

        public List<UserNotification> GetForUser(string userJmbg)
        {
            return GetAll().FindAll(x => x.patientID == userJmbg);
        }
    }
}