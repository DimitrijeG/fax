using System.Collections.ObjectModel;
using HealthCare.Application;
using HealthCare.Core.Interior;

namespace HealthCare.WPF.ManagerGUI.Interior.Renovations
{
    public class RenovationListingViewModel
    {
        public RenovationListingViewModel()
        {
            Items = new ObservableCollection<RoomViewModel>();
            LoadAll();
        }

        public ObservableCollection<RoomViewModel> Items { get; }

        public void LoadAll()
        {
            var rooms = Injector.GetService<RoomService>().GetAll();
            foreach (var room in rooms)
                Items.Add(new RoomViewModel(room));
        }
    }
}