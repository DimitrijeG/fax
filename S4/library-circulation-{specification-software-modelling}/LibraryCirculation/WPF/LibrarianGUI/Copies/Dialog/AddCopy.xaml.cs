using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LibraryCirculation.Application;
using LibraryCirculation.Application.Exceptions;
using LibraryCirculation.Core.Fund;
using LibraryCirculation.Core.Fund.Books;
using LibraryCirculation.Core.Fund.Organization;
using LibraryCirculation.WPF.Common;

namespace LibraryCirculation.WPF.LibrarianGUI.Copies.Dialog
{
    public partial class AddCopy
    {
        public AddCopy()
        {
            InitializeComponent();
            Books = new ObservableCollection<Book>(Injector.GetService<BookService>().GetAll());
            Branches = new ObservableCollection<Branch>(Injector.GetService<BranchService>().GetAll());
            InventoryBooks = new ObservableCollection<InventoryBook>();
            DataContext = this;
        }

        public ObservableCollection<Book> Books { get; set; }
        public ObservableCollection<Branch> Branches { get; set; }
        public ObservableCollection<InventoryBook> InventoryBooks { get; set; }

        private void Create_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (BookPicker.SelectedValue is null ||
                InventoryBookPicker.SelectedValue is null ||
                PriceTextBox.Text == "")
            {
                ViewUtil.ShowWarning("Molimo izaberite/popunite sva polja.");
                return;
            }

            try
            {
                var price = double.Parse(PriceTextBox.Text);
                if (price < 0)
                    throw new ValidationException();

                var copy = new Copy
                {
                    Price = price,
                    InventoryBookId = (int)InventoryBookPicker.SelectedValue,
                    BookIsbn = (string)BookPicker.SelectedValue
                };

                Injector.GetService<CopyService>().Add(copy);
                ViewUtil.ShowInformation("Dodavanje primerka uspešno.");
                Close();
            }
            catch
            {
                ViewUtil.ShowWarning("Cena mora da bude pozitivan broj.");
            }
        }

        private void Cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BranchPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BranchPicker.SelectedItem == null) return;

            var id = (int)BranchPicker.SelectedValue;
            InventoryBooks.Clear();
            Injector.GetService<InventoryBookService>()
                .GetForBranch(id)
                .ForEach(ib => InventoryBooks.Add(ib));
        }
    }
}