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

namespace LibraryCirculation.WPF.ReaderGUI.Reservations
{
    public partial class CreateReservationView : Window
    {
        public CreateReservationView()
        {
            InitializeComponent();
            DataContext = new CreateReservationViewModel(this);
        }
    }
}
