using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryCirculation.Core.Fund.Books;
using LibraryCirculation.WPF.LibrarianGUI.Books;

namespace LibraryCirculation.WPF.DefaultGUI
{
    public class BookRatingViewModel : BookViewModel
    {
        public int Rating { get; }
        public BookRatingViewModel(Book book, int rating) : base(book)
        {
            Rating = rating;
        }
    }
}
