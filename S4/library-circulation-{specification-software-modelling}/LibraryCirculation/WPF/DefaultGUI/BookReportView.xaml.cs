using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LibraryCirculation.WPF.DefaultGUI
{
    public partial class BookReportView : Window
    {
        public BookReportView()
        {
            InitializeComponent();
            DataContext = new BookReportViewModel(this);
        }
    }
}
