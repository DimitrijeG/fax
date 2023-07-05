using System.Collections.ObjectModel;
using System.Windows;
using LibraryCirculation.Application;
using LibraryCirculation.Core.Fund;
using LibraryCirculation.WPF.Common;
using LibraryCirculation.WPF.LibrarianGUI.Copies.Dialog;

namespace LibraryCirculation.WPF.LibrarianGUI.Copies
{
    public partial class CopyManagementView
    {
        public CopyManagementView()
        {
            InitializeComponent();
            Copies = new ObservableCollection<Copy>(Injector.GetService<CopyService>().GetAll());
            DataContext = this;
        }

        public ObservableCollection<Copy> Copies { get; set; }
        public Copy? SelectedCopy { get; set; }

        private void Create_Btn_Click(object sender, RoutedEventArgs e)
        {
            new AddCopy().ShowDialog();
            UpdateCollection();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCopy is null) return;

            var dialogResult = ViewUtil.ShowConfirmation("Da li ste sigurni da želite da obrišete primerak?");
            if (dialogResult != MessageBoxResult.Yes) return;

            ViewUtil.ShowInformation("Brisanje uspešno izvršeno.");
            Injector.GetService<CopyService>().Remove(SelectedCopy.GetKey());
            UpdateCollection();
        }

        private void UpdateCollection()
        {
            Copies.Clear();
            foreach (var copy in Injector.GetService<CopyService>().GetAll())
                Copies.Add(copy);
        }
    }
}