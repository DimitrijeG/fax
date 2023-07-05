using System.Collections.ObjectModel;
using HealthCare.Application;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.Core.PhysicalAssets;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.NurseGUI.PatientHealthcare.Pharmacy
{
    public class MedicationOrderListingViewModel : ViewModelBase
    {
        private readonly InventoryService _inventoryService;
        private readonly MedicationService _medicationService;

        public MedicationOrderListingViewModel()
        {
            _medicationService = Injector.GetService<MedicationService>();
            _inventoryService = Injector.GetService<InventoryService>(Injector.MEDICATION_INVENTORY_S);

            Items = new ObservableCollection<OrderMedicationViewModel>();
            LoadAll();
        }

        public ObservableCollection<OrderMedicationViewModel> Items { get; }

        public void LoadAll()
        {
            Items.Clear();
            foreach (int id in _inventoryService.GetLowQuantityEquipment(5))
            {
                var medication = _medicationService.Get(id);
                var quantity = _inventoryService.GetTotalQuantity(id);
                Items.Add(new OrderMedicationViewModel(medication, quantity));
            }
        }
    }
}