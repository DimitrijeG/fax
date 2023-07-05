namespace LibraryCirculation.WPF.LibrarianGUI.Books.Dialog
{
    public partial class AddEditBookView
    {
        public AddEditBookView(string? isbn = null)
        {
            InitializeComponent();
            DataContext = new AddEditBookViewModel(this, isbn);
        }
    }
}