using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LibraryCirculation.Application;
using LibraryCirculation.Application.Exceptions;
using LibraryCirculation.Core.Fund.Books;
using LibraryCirculation.WPF.Common;

namespace LibraryCirculation.WPF.LibrarianGUI.Books.Dialog
{
    public class AddEditBookViewModel : ViewModelBase
    {
        private readonly BookService _bookService;

        public AddEditBookViewModel(Window window, string? isbn = null)
        {
            _bookService = Injector.GetService<BookService>();
            Book = isbn is null ? new Book() : _bookService.Get(isbn);
            Adding = isbn is null;
            Title = Adding ? "Dodavanje naslova" : "Izmena naslova";
            CommandTitle = Adding ? "Dodaj" : "Izmeni";

            Years = GetValidYears();
            Publishers = Injector.GetService<PublisherService>().GetAll();
            Authors = string.Join(" ", Book.AuthorIds);
            Formats = GetFormatNames();
            SelectedFormat = GetFormatNames().FindIndex(n => n == ViewUtil.Translate(Book.Format));
            CoverTypes = new List<string> { "meko", "tvrdo" };
            SelectedCover = Book.Hardcover ? 1 : 0;

            ExitCommand = new ExitCommand(window);
            CommitCommand = GetCommitCommand();
            AuthorsCommand = new RelayCommand(_ => { new AuthorsListingView().ShowDialog(); });
        }

        public Book Book { get; }
        public bool Adding { get; }
        public string Title { get; }

        public List<int> Years { get; }
        public List<Publisher> Publishers { get; }
        public string Authors { get; set; }
        public List<string> Formats { get; }
        public int SelectedFormat { get; set; }
        public List<string> CoverTypes { get; }
        public int SelectedCover { get; set; }

        public ExitCommand ExitCommand { get; }
        public string CommandTitle { get; }
        public RelayCommand CommitCommand { get; }
        public RelayCommand AuthorsCommand { get; }

        private RelayCommand GetCommitCommand()
        {
            return new RelayCommand(_ =>
            {
                try
                {
                    Validate();
                }
                catch (ValidationException ve)
                {
                    ViewUtil.ShowWarning(ve.Message);
                    return;
                }

                if (Adding) _bookService.Add(Book);
                else _bookService.Update(Book);
                ViewUtil.ShowInformation($"Naslov uspešno {(Adding ? "dodat" : "izmenjen")}.");
                ExitCommand.Execute();
            });
        }

        private static List<int> GetValidYears()
        {
            var years = new List<int>();
            var i = DateTime.Now.Year;
            while (i > 1600) years.Add(i--);
            return years;
        }

        private static List<string> GetFormatNames()
        {
            return Enum.GetValues<BookFormat>().Select(ViewUtil.Translate).ToList();
        }

        private void Validate()
        {
            if (Adding && _bookService.Contains(Book.Isbn))
                throw new ValidationException("Naslov sa unetim isbn-om već postoji.");

            List<int> authorIds;
            try
            {
                authorIds = Authors.Split().Select(s => int.Parse(s.Trim())).ToList();
            }
            catch (FormatException)
            {
                throw new ValidationException("Format id-jeva autora nije ispravan.");
            }

            if (!authorIds.Any())
                throw new ValidationException("Naslov mora da ima bar jednog autora.");
            if (!authorIds.All(id => Injector.GetService<AuthorService>().Contains(id)))
                throw new ValidationException("Autor sa unetim id-jem ne postoji.");
        }
    }
}