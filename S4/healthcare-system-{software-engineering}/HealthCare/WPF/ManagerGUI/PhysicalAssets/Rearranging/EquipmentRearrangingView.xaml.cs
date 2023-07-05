using System;
using System.Windows;
using HealthCare.Application;
using HealthCare.Application.Exceptions;
using HealthCare.Core.Interior;
using HealthCare.Core.PhysicalAssets;
using HealthCare.WPF.Common;
using HealthCare.WPF.ManagerGUI.PhysicalAssets.Inventory;

namespace HealthCare.WPF.ManagerGUI.PhysicalAssets.Rearranging
{
    public partial class EquipmentRearrangingView : Window
    {
        private readonly TransferService _transferService;
        private RearrangingViewModel _model;
        private Equipment? _selected;

        public EquipmentRearrangingView()
        {
            InitializeComponent();
            _transferService = Injector.GetService<TransferService>();

            _model = new RearrangingViewModel();
            DataContext = _model;
        }

        private void cb_SelectionChanged(object sender, EventArgs e)
        {
            _selected = cbEquipment.SelectedItem as Equipment;
            if (_selected is null) return;

            if (_selected.IsDynamic)
                datePicker.IsEnabled = false;
            else datePicker.IsEnabled = true;
            _model.Load(_selected);
        }

        private void Button_Transfer(object sender, RoutedEventArgs e)
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

            if (_selected is null) return;

            CreateTransfer();
            ViewUtil.ShowInformation("Uspešna operacija.");

            tbQuantity.Text = "";
            datePicker.SelectedDate = null;
            _model.Load(_selected);
        }

        private void CreateTransfer()
        {
            var equipment = ((Equipment)cbEquipment.SelectedItem).Id;
            var quantity = int.Parse(tbQuantity.Text.Trim());
            var date = datePicker.SelectedDate;
            var from = ((InventoryItemViewModel)lvFromRoom.SelectedItem).Room.Id;
            var to = ((InventoryItemViewModel)lvToRoom.SelectedItem).Room.Id;

            var transfer = new TransferItem(equipment, quantity, DateTime.Now, false, from, to);
            if (date is null)
            {
                _transferService.Execute(transfer);
                return;
            }

            transfer.Scheduled = (DateTime)date;
            _transferService.Add(transfer);
        }

        private void Validate()
        {
            int quantity;
            if (_selected is null)
                throw new ValidationException("Izaberite opremu za prenos.");
            else if (!(int.TryParse(tbQuantity.Text.Trim(), out quantity) && quantity > 0))
                throw new ValidationException("Količina opreme za prenos mora da bude pozitivan broj.");

            var from = (InventoryItemViewModel)lvFromRoom.SelectedItem;
            var to = (InventoryItemViewModel)lvToRoom.SelectedItem;
            if (from is null || to is null)
                throw new ValidationException("Izaberite sobe za prenos iz obe tabele.");
            else if (from.Room.Id == to.Room.Id)
                throw new ValidationException("Prenos opreme iz sobe u nju samu nije moguć.");
            else if (from.Quantity < quantity)
                throw new ValidationException("Nema dovoljno opreme da bi se izvršio prenos.");

            var date = datePicker.SelectedDate;
            if (!_selected.IsDynamic && date is null)
                throw new ValidationException("Pošto oprema nije dinamička obavezno je izabrati datum prenosa.");
            else if (!_selected.IsDynamic && date <= DateTime.Now)
                throw new ValidationException("Datum prenosa ne sme da bude u prošlosti.");
        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}