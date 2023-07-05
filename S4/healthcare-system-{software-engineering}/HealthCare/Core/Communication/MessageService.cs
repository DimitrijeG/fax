using System.Collections.Generic;
using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Communication
{
    public class MessageService : NumericService<Message>
    {
        public MessageService(IRepository<Message> repository) : base(repository)
        {
        }

        public List<Message> GetByContact(int contactID)
        {
            return GetAll().FindAll(x => x.contactID == contactID);
        }
    }
}