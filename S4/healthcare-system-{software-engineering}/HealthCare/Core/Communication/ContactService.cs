using System.Collections.Generic;
using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Communication
{
    public class ContactService : NumericService<Contact>
    {
        public ContactService(IRepository<Contact> repository) : base(repository)
        {
        }

        public List<Contact> GetForUser(string userJmbg)
        {
            return GetAll().FindAll(x => x.Participants.Contains(userJmbg));
        }
    }
}