using System.Collections.Generic;
using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Notifications
{
    public class NotificationService : NumericService<Notification>
    {
        public NotificationService(IRepository<Notification> repository) : base(repository)
        {
        }

        public List<Notification> GetForUser(string userJmbg)
        {
            return GetAll().FindAll(x => !x.Seen && x.Recipients.Contains(userJmbg));
        }
    }
}