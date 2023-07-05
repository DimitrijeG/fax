using System;
using System.Windows;
using System.Windows.Controls;
using HealthCare.Application;
using HealthCare.Application.Exceptions;
using HealthCare.Core.PhysicalAssets;
using HealthCare.WPF.Common;
using Validation = HealthCare.WPF.Common.Validation;

namespace HealthCare.WPF.NurseGUI.PatientHealthcare.Pharmacy
{
    public partial class MedicationOrderView
    {
        private readonly OrderService _medicationOrderService;
        private MedicationOrderListingViewModel _model;

        public MedicationOrderView()
        {
            InitializeComponent();
            _model = new MedicationOrderListingViewModel();
            DataContext = _model;

            _medicationOrderService = Injector.GetService<OrderService>(Injector.MEDICATION_ORDER_S);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
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
                    MakeOrder(item.Id, int.Parse(item.OrderQuantity));

            ViewUtil.ShowInformation("Poručivanje uspešno.");
            _model.LoadAll();
        }

        private void MakeOrder(int medicationID, int quantity)
        {
            var scheduled = DateTime.Now + new TimeSpan(24, 0, 0);
            _medicationOrderService.Add(new OrderItem(medicationID, quantity, scheduled, false));
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
    }
}