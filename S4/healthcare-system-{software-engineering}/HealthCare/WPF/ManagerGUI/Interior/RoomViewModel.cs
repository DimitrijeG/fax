using HealthCare.Core.Interior;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.ManagerGUI.Interior
{
    public class RoomViewModel
    {
        private readonly Room _room;

        public RoomViewModel(Room room)
        {
            _room = room;
        }

        public int Id => _room.Id;
        public string Name => _room.Name;
        public string Type => ViewUtil.Translate(_room.Type);
    }
}