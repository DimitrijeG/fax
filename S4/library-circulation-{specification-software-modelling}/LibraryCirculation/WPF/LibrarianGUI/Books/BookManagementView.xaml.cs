using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LibraryCirculation.Application;
using LibraryCirculation.Application.Exceptions;
using LibraryCirculation.Core.Fund.Books;
using LibraryCirculation.WPF.Common;
using LibraryCirculation.WPF.LibrarianGUI.Books.Dialog;

namespace LibraryCirculation.WPF.LibrarianGUI.Books
{
    public partial class BookManagementView
    {
        private readonly BookManagementViewModel _viewModel;

        public BookManagementView()
        {
            InitializeComponent();

            _viewModel = new BookManagementViewModel();
            DataContext = _viewModel;
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            new AddEditBookView().ShowDialog();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selected = GetSelected();
            try
            {
                Validate(true);
            }
            catch (ValidationException ve)
            {
                ViewUtil.ShowWarning(ve.Message);
                return;
            }

            new AddEditBookView(selected.First()).ShowDialog();
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            var bookService = Injector.GetService<BookService>();
            var selected = GetSelected();
            try
            {
                Validate();
            }
            catch (ValidationException ve)
            {
                ViewUtil.ShowWarning(ve.Message);
                return;
            }

            var response = ViewUtil.ShowConfirmation(
                $"Da li ste sigurni da želite da obrišete naslove? ({selected.Count})");
            if (response != MessageBoxResult.Yes) return;

            selected.ForEach(isbn => bookService.Remove(isbn));
            ViewUtil.ShowInformation("Brisanje uspešno.");
            _viewModel.LoadAll();
        }

        private List<string> GetSelected()
        {
            return (from BookViewModel model in LvBooks.SelectedItems select model.Isbn).ToList();
        }

        private void Validate(bool edit = false)
        {
            var selected = GetSelected();
            const string message = "Molimo izaberite naslov";
            if (!selected.Any())
                throw new ValidationException(message + (edit ? " za izmenu." : "/e za brisanje."));
            if (edit && selected.Count > 1)
                throw new ValidationException("Izaberite jedan naslov za izmenu.");
        }

        private void BtnNotImplemented_Click(object sender, RoutedEventArgs e)
        {
            ViewUtil.ShowInformation("Funkcionalnost nije implementirana.");
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}