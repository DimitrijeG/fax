using System;
using System.Linq;
using System.Windows;
using HealthCare.Application;
using HealthCare.Application.Exceptions;
using HealthCare.Core.Interior.Renovation.Model;
using HealthCare.Core.Interior.Renovation.Service;
using HealthCare.Core.Scheduling;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Scheduling.Schedules;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.ManagerGUI.Interior.Renovations
{
    public partial class RenovationListingView : Window
    {
        private readonly BasicRenovationService _basicRenovationService;
        private readonly RoomSchedule _roomSchedule;

        public RenovationListingView()
        {
            InitializeComponent();

            _basicRenovationService = Injector.GetService<BasicRenovationService>();
            _roomSchedule = Injector.GetService<RoomSchedule>();

            DataContext = new RenovationListingViewModel();

            btnRenovate.IsEnabled = true;
            btnJoin.IsEnabled = false;
            btnSplit.IsEnabled = false;
        }

        private void Validate()
        {
            var selected = lvRooms.SelectedItems;

            if (selected.Count == 0)
                throw new ValidationException("Izaberite sobu/sobe za renoviranje.");

            var startDate = StartDatePicker.SelectedDate;
            var endDate = EndDatePicker.SelectedDate;

            if (startDate is null || endDate is null)
                throw new ValidationException("Nisu izabrani datum pocetka i kraja renoviranja.");

            if (startDate <= DateTime.Now)
                throw new ValidationException("Datum početka renoviranja mora da bude u budućnosti.");

            if (endDate <= startDate)
                throw new ValidationException("Datum kraja renoviranja ne može da bude pre početka.");

            var slot = new TimeSlot((DateTime)startDate, (DateTime)endDate);

            foreach (RoomViewModel room in selected)
                ValidateRoom(room, slot);
        }


        private void ValidateRoom(RoomViewModel room, TimeSlot slot)
        {
            var id = room.Id;
            Appointment appointment = new Appointment("", "", slot, false);
            appointment.RoomID = id;
            if (!_roomSchedule.IsAvailable(appointment))
                throw new ValidationException(
                    $"Soba sa ID-jem {id} nije slobodna u izabranom terminu.");
        }

        private bool IsValid()
        {
            try
            {
                Validate();
            }
            catch (ValidationException ve)
            {
                ViewUtil.ShowWarning(ve.Message);
                return false;
            }

            return true;
        }

        private TimeSlot GetScheduled()
        {
            var start = StartDatePicker.SelectedDate ?? default;
            var end = EndDatePicker.SelectedDate ?? default;
            return new TimeSlot(start, end);
        }

        private void Reset()
        {
            lvRooms.SelectedItems.Clear();
            StartDatePicker.SelectedDate = null;
            StartDatePicker.SelectedDate = null;
        }

        private void btnRenovate_Click(object sender, RoutedEventArgs e)
        {
            if (!IsValid()) return;

            var roomId = ((RoomViewModel)lvRooms.SelectedItem).Id;
            var scheduled = GetScheduled();

            _basicRenovationService.Add(new RenovationBase(roomId, scheduled));

            ViewUtil.ShowInformation("Uspesno zakazano renoviranje sobe.");
            Reset();
        }

        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            if (!IsValid()) return;

            var rooms = lvRooms.SelectedItems.Cast<RoomViewModel>();
            var scheduled = GetScheduled();

            new JoiningRenovationView(rooms.ToList(), scheduled).ShowDialog();
            Reset();
        }

        private void btnSplit_Click(object sender, RoutedEventArgs e)
        {
            if (!IsValid()) return;

            var roomModel = (RoomViewModel)lvRooms.SelectedItem;
            var scheduled = GetScheduled();

            new SplittingRenovationView(roomModel, scheduled).ShowDialog();
            Reset();
        }

        private void lvRooms_SelectionChanged(object sender, EventArgs e)
        {
            var selected = lvRooms.SelectedItems;
            int maxSelections = cbComplex.IsChecked ?? false ? 2 : 1;

            while (selected.Count > maxSelections)
                selected.RemoveAt(selected.Count - 1);

            btnJoin.IsEnabled = selected.Count == 2;
            btnSplit.IsEnabled = (cbComplex.IsChecked ?? false)
                                 && selected.Count < 2;
        }

        private void cbComplex_Checked(object sender, RoutedEventArgs e)
        {
            btnRenovate.IsEnabled = false;
            btnSplit.IsEnabled = true;
        }

        private void cbComplex_Unchecked(object sender, RoutedEventArgs e)
        {
            btnRenovate.IsEnabled = true;
            btnSplit.IsEnabled = false;
            lvRooms_SelectionChanged(sender, e);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}