using System;
using System.Collections.Generic;
using System.Windows;
using HealthCare.Application;
using HealthCare.Core.Interior;
using HealthCare.Core.Interior.Renovation.Model;
using HealthCare.Core.Interior.Renovation.Service;
using HealthCare.Core.Scheduling;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.ManagerGUI.Interior.Renovations
{
    public partial class JoiningRenovationView
    {
        private readonly JoiningRenovationService _joiningRenovationService;
        private readonly int _room1, _room2;
        private readonly TimeSlot _scheduled;

        public JoiningRenovationView(List<RoomViewModel> rooms, TimeSlot scheduled)
        {
            InitializeComponent();

            _joiningRenovationService = Injector.GetService<JoiningRenovationService>();

            _scheduled = scheduled;
            _room1 = rooms[0].Id;
            _room2 = rooms[1].Id;

            lvRooms.ItemsSource = rooms;
            InitializeComboBox();
        }

        private void InitializeComboBox()
        {
            foreach (RoomType a in Enum.GetValues(typeof(RoomType)))
                cbType.Items.Add(ViewUtil.Translate(a));
            cbType.SelectedIndex = 0;
        }

        private void btnRenovate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                ViewUtil.ShowWarning("Naziv sobe ne sme da bude prazan.");
                return;
            }

            var name = tbName.Text.Trim();
            var typeIndex = cbType.SelectedIndex;
            var resultRoom = new Room(0, name, (RoomType)typeIndex);

            _joiningRenovationService.Add(new JoiningRenovation(_room1, _scheduled, _room2, resultRoom));
            ViewUtil.ShowInformation("Uspesno zakazano renoviranje 2 sobe.");
            Close();
        }

        private void btnExist_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}