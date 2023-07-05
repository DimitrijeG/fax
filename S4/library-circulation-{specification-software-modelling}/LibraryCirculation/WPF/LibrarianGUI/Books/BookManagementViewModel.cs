using System.Collections.ObjectModel;
using LibraryCirculation.Application;
using LibraryCirculation.Core.Fund.Books;
using LibraryCirculation.WPF.Common;

namespace LibraryCirculation.WPF.LibrarianGUI.Books
{
    public class BookManagementViewModel : ViewModelBase
    {
        public BookManagementViewModel()
        {
            Items = new ObservableCollection<BookViewModel>();

            LoadAll();
        }

        public ObservableCollection<BookViewModel> Items { get; }

        public void LoadAll()
        {
            Items.Clear();
            Injector.GetService<BookService>()
                .GetAll().ForEach(b => Items.Add(new BookViewModel(b)));
        }
    }
}