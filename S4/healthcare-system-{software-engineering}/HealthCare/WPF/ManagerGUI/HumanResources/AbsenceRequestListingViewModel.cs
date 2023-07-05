using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using HealthCare.Application;
using HealthCare.Application.Exceptions;
using HealthCare.Core.HumanResources;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.ManagerGUI.HumanResources
{
    public class AbsenceRequestListingViewModel : ViewModelBase
    {
        private readonly AbsenceRequestService _absenceRequestService;
        private bool _areApproved = false;

        public AbsenceRequestListingViewModel(Window view)
        {
            Items = new ObservableCollection<AbsenceRequestViewModel>();
            _absenceRequestService = Injector.GetService<AbsenceRequestService>();

            ApproveRequestCommand = new ManageRequestCommand(this, true);
            DeclineRequestCommand = new ManageRequestCommand(this, false);
            ExitCommand = new CancelCommand(view);
            LoadAll();
        }

        public ICommand ApproveRequestCommand { get; }
        public ICommand DeclineRequestCommand { get; }
        public ICommand ExitCommand { get; }
        public ObservableCollection<AbsenceRequestViewModel> Items { get; }

        public bool AreApproved
        {
            get => _areApproved;
            set
            {
                _areApproved = value;
                LoadAll();
            }
        }

        public AbsenceRequestViewModel? SelectedRequest { get; set; }

        public void LoadAll()
        {
            Items.Clear();
            _absenceRequestService
                .GetAll().Where(r => !r.IsApproved || AreApproved)
                .OrderBy(r => !r.IsApproved).ThenBy(r => r.Id)
                .Select(r => new AbsenceRequestViewModel(r))
                .ToList().ForEach(model => Items.Add(model));
        }

        public AbsenceRequest TryGetSelectedRequest(bool approve)
        {
            if (SelectedRequest is null)
                throw new ValidationException("Molimo izaberite zahtev za odsustvo.");

            var request = _absenceRequestService.Get(SelectedRequest.Id);

            if (request.IsApproved)
                throw new ValidationException("Izabrani zahtev je već odobren.");

            if (approve && _absenceRequestService.OverlappingOther(request))
                throw new ValidationException("Izabrani zahtev se preklapa sa drugim odobrenim zahtevom za odsustvo.");


            return request;
        }
    }
}