using System.Collections.Generic;
using System.Linq;
using LibraryCirculation.Application;
using LibraryCirculation.Core.Fund.Books;
using LibraryCirculation.WPF.Common;

namespace LibraryCirculation.WPF.LibrarianGUI.Books
{
    public class BookViewModel
    {
        private readonly Book _book;

        public BookViewModel(Book book)
        {
            _book = book;
            var publisherSv = Injector.GetService<PublisherService>();
            var authorSv = Injector.GetService<AuthorService>();

            Publisher = publisherSv.Get(_book.PublisherId).DisplayName;
            Authors = _book.AuthorIds
                .Select(id => authorSv.Get(id).DisplayName)
                .ToList();
        }

        public string Isbn => _book.Isbn;
        public string Udk => _book.Udk;
        public string Title => _book.Title;
        public int Year => _book.Year;
        public string Language => _book.Language;
        public string Publisher { get; }
        public List<string> Authors { get; }
        public string Format => ViewUtil.Translate(_book.Format);
        public string CoverType => _book.Hardcover ? "tvrdo" : "meko";
    }
}