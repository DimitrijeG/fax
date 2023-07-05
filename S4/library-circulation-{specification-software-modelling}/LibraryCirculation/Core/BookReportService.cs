using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryCirculation.Application;
using LibraryCirculation.Core.Common;
using LibraryCirculation.Core.Fund;
using LibraryCirculation.Core.Fund.Books;
using LibraryCirculation.Core.Renting;

namespace LibraryCirculation.Core
{
    public class BookReportService
    {
        private readonly CopyService _copyService;

        public BookReportService()
        {
            _copyService = Injector.GetService<CopyService>();
        }

        private Dictionary<int, int> GetRentedCopiesCounts(TimeSlot timeSlot)
        {
            var copyCount = new Dictionary<int, int>();
            _copyService.GetAll().ForEach(c => copyCount[c.Id] = 0);

            Injector.GetService<RentalService>()
                .GetAll().Where(r => timeSlot.Contains(r.Time.Start))
                .Select(r => r.CopyId).ToList()
                .ForEach(id => ++copyCount[id]);
            return copyCount;
        }

        public List<Book> GetReport(TimeSlot timeSlot, int count = 10)
        {
            var copyService = Injector.GetService<CopyService>();
            var rentedCounts = GetRentedCopiesCounts(timeSlot);

            return Injector.GetService<BookService>()
                .GetAll().OrderByDescending(b => 
                    copyService.GetForBook(b.Isbn)
                    .Sum(c => rentedCounts[c.Id])
                ).Take(count).ToList();
        }
    }
}
