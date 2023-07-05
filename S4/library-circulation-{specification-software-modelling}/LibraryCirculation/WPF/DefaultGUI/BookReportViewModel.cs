using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using LibraryCirculation.Application;
using LibraryCirculation.Application.Common;
using LibraryCirculation.Core;
using LibraryCirculation.Core.Common;
using LibraryCirculation.Core.Fund.Books;
using LibraryCirculation.WPF.Common;

namespace LibraryCirculation.WPF.DefaultGUI
{
    public struct RatingCategory
    {
        public string Title { get; set; }
        public TimeSlot TimeSlot { get; set; }

        public RatingCategory(string title, TimeSlot timeSlot)
        {
            Title = title;
            TimeSlot = timeSlot;
        }
    }

    public class BookReportViewModel : ViewModelBase
    {
        public ObservableCollection<BookRatingViewModel> Items { get; }
        public List<RatingCategory> Categories { get; }

        private TimeSlot _selectedTimeSlot;
        public TimeSlot SelectedTimeSlot
        {
            get => _selectedTimeSlot;
            set
            {
                _selectedTimeSlot = value;
                FilterRatings();
            }
        }

        public ExitCommand ExitCommand { get; }

        public BookReportViewModel(Window current)
        {
            Items = new ObservableCollection<BookRatingViewModel>();

            Categories = new List<RatingCategory>
            {
                new(
                    "nedelju dana", 
                    new TimeSlot(DateTime.Now.AddDays(-7), DateTime.Now)
                ),
                new(
                    "mesec dana",
                    new TimeSlot(DateTime.Now.AddMonths(-1), DateTime.Now)
                ),
                new(
                    "godinu dana",
                    new TimeSlot(DateTime.Now.AddYears(-1), DateTime.Now)
                ),
            };
            ExitCommand = new ExitCommand(current);

            _selectedTimeSlot = Categories.ElementAt(0).TimeSlot;
            FilterRatings();
        }

        private void FilterRatings()
        {
            Items.Clear();
            var books = Injector.GetService<BookReportService>()
                .GetReport(SelectedTimeSlot);
            
            foreach (var (b, i) in Util.WithIndex(books))
                Items.Add(new BookRatingViewModel(b, i+1));
        }
    }
}
