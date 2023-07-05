using LibraryCirculation.Application;
using LibraryCirculation.Core.Fund.Books;

namespace LibraryCirculation.WPF.LibrarianGUI.Books.Dialog
{
    public partial class AuthorsListingView
    {
        public AuthorsListingView()
        {
            InitializeComponent();
            LvAuthors.ItemsSource = Injector.GetService<AuthorService>().GetAll();
        }
    }
}