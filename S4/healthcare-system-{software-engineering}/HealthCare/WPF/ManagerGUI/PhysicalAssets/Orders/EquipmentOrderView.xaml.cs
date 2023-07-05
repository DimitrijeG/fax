using System;
using System.Windows;
using System.Windows.Controls;
using HealthCare.Application;
using HealthCare.Application.Exceptions;
using HealthCare.Core.PhysicalAssets;
using HealthCare.WPF.Common;
using Validation = HealthCare.WPF.Common.Validation;

namespace HealthCare.WPF.ManagerGUI.PhysicalAssets.Orders
{
    public partial class EquipmentOrderView : Window
    {
        private readonly OrderService _orderService;
        private EquipmentOrderViewModel _model;

        public EquipmentOrderView()
        {
            InitializeComponent();
            _orderService = Injector.GetService<OrderService>(Injector.EQUIPMENT_ORDER_S);

            _model = new EquipmentOrderViewModel();
            DataContext = _model;
        }

        private void Button_Reset(object sender, RoutedEventArgs e)
        {
            _model.LoadAll();
        }

        private void Button_Order(object sender, RoutedEventArgs e)
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

            foreach (var item in _model.Items)
                if (item.IsSelected)
                    MakeOrder(item.EquipmentId, int.Parse(item.OrderQuantity));

            ViewUtil.ShowInformation("Poručivanje uspešno.");
            _model.LoadAll();
        }

        private void MakeOrder(int equipmentId, int quantity)
        {
            var scheduled = DateTime.Now + new TimeSpan(24, 0, 0);
            _orderService.Add(new OrderItem(equipmentId, quantity, scheduled, false));
        }

        private void Validate()
        {
            bool someSelected = false;
            foreach (var item in _model.Items)
            {
                if (item.IsSelected && !Validation.IsNatural(item.OrderQuantity))
                    throw new ValidationException("Količina mora da bude prirodan broj.");

                someSelected |= item.IsSelected;
            }

            if (!someSelected)
                throw new ValidationException("Nema unetih porudžbina.");
        }

        private void tbQuantity_Focused(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if (tb is null) return;
            if (tb.Text == "0")
                tb.Text = "";
        }

        private void tbQuantity_Unfocused(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if (tb is null) return;
            if (tb.Text == "")
                tb.Text = "0";
        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}