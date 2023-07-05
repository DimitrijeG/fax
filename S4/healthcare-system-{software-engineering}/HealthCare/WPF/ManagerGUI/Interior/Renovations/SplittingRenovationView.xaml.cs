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
    public partial class SplittingRenovationView : Window
    {
        private readonly int _roomId;
        private readonly TimeSlot _scheduled;
        private readonly SplittingRenovationService _splittingRenovationService;

        public SplittingRenovationView(RoomViewModel roomModel, TimeSlot scheduled)
        {
            InitializeComponent();

            _scheduled = scheduled;
            _roomId = roomModel.Id;

            lvRooms.ItemsSource = new List<RoomViewModel> { roomModel };
            _splittingRenovationService = Injector.GetService<SplittingRenovationService>();
            InitializeComboBoxes();
        }

        private void InitializeComboBoxes()
        {
            foreach (RoomType a in Enum.GetValues(typeof(RoomType)))
            {
                cbType1.Items.Add(ViewUtil.Translate(a));
                cbType2.Items.Add(ViewUtil.Translate(a));
            }

            cbType1.SelectedIndex = 0;
            cbType2.SelectedIndex = 0;
        }

        private void btnRenovate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbName1.Text) || string.IsNullOrWhiteSpace(tbName2.Text))
            {
                ViewUtil.ShowWarning("Naziv sobe ne sme da bude prazan.");
                return;
            }

            var name1 = tbName1.Text.Trim();
            var typeIndex1 = cbType1.SelectedIndex;
            var name2 = tbName2.Text.Trim();
            var typeIndex2 = cbType2.SelectedIndex;

            var room1 = new Room(0, name1, (RoomType)typeIndex1);
            var room2 = new Room(0, name2, (RoomType)typeIndex2);

            _splittingRenovationService.Add(new SplittingRenovation(_roomId, _scheduled, room1, room2));
            ViewUtil.ShowInformation("Uspesno zakazano renoviranje sobe.");
            Close();
        }

        private void btnExist_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}