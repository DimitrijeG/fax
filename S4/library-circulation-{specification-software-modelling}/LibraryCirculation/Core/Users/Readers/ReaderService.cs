using LibraryCirculation.Core.Common;
using LibraryCirculation.DataManagement.Repository;

namespace LibraryCirculation.Core.Users.Readers
{
    public class ReaderService : CrudService<Reader>
    {
        public ReaderService(IRepository<Reader> repository) : base(repository)
        {
        }

        public new void Add(Reader item)
        {
            if (item.CardId == 0)
                item.CardId = GetNextCardId();

            if (ContainsCardId(item.CardId)) return;
            base.Add(item);
        }

        public new void Update(Reader item)
        {
            if (ContainsCardId(item.CardId)) return;
            base.Update(item);
        }

        public bool ContainsCardId(int cardId)
        {
            return GetAll().Find(r => r.CardId == cardId) != null;
        }

        public int GetNextCardId()
        {
            var i = 1;
            while (ContainsCardId(i)) ++i;
            return i;
        }
    }
}