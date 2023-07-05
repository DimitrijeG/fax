using System;
using System.Collections.ObjectModel;
using System.Windows;
using HealthCare.Application;
using HealthCare.Core.Interior;
using HealthCare.Core.PatientHealthcare.HealthcareTreatment;
using HealthCare.Core.Scheduling;
using HealthCare.Core.Scheduling.Schedules;
using HealthCare.Core.Users.Model;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;
using HealthCare.WPF.ManagerGUI.Interior;

namespace HealthCare.WPF.NurseGUI.PatientHealthcare.Referrals.Treatment
{
    public class TreatmantReferralListingViewModel : ViewModelBase
    {
        private readonly TreatmentReferralService _treatmentReferralService;

        private Patient _patient;
        private TreatmentReferralViewModel _selected;

        private Core.PatientHealthcare.HealthcareTreatment.Treatment _treatment;

        public TreatmantReferralListingViewModel(Patient patient, Window window)
        {
            _patient = patient;
            _treatmentReferralService = Injector.GetService<TreatmentReferralService>();
            Rooms = new ObservableCollection<RoomViewModel>();
            Referrals = new ObservableCollection<TreatmentReferralViewModel>();

            cancelCommand = new CancelCommand(window);
            makeTreatmantCommand = new RelayCommand(o =>
            {
                if (_treatment is null || SelectedRoom is null)
                {
                    ViewUtil.ShowWarning("Izaberite uput i sobu");
                    return;
                }

                _treatment.RoomId = SelectedRoom.Id;
                Injector.GetService<TreatmentService>().Add(_treatment);
                var updated = _treatmentReferralService.Get(_treatment.ReferralId);
                updated.IsUsed = true;
                _treatmentReferralService.Update(updated);
                loadReferrals();
            });

            loadReferrals();
        }

        public ObservableCollection<TreatmentReferralViewModel> Referrals { get; }

        public TreatmentReferralViewModel Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                if (_selected is not null)
                {
                    _treatment = new Core.PatientHealthcare.HealthcareTreatment.Treatment(0, value.Id,
                        new TimeSlot(DateTime.Now, new TimeSpan(value.Days, 0, 0, 0)));
                    loadRooms(_treatment);
                }
            }
        }

        public CancelCommand cancelCommand { get; set; }
        public RelayCommand makeTreatmantCommand { get; set; }

        public ObservableCollection<RoomViewModel> Rooms { get; }
        public RoomViewModel? SelectedRoom { get; set; }

        public void loadReferrals()
        {
            Referrals.Clear();
            Rooms.Clear();

            _treatmentReferralService
                .GetPatientsReferrals(_patient.JMBG)
                .ForEach(r => Referrals.Add(new TreatmentReferralViewModel(r)));
        }

        public void loadRooms(Core.PatientHealthcare.HealthcareTreatment.Treatment treatment)
        {
            var roomService = Injector.GetService<RoomService>();
            Rooms.Clear();

            Injector.GetService<RoomSchedule>()
                .GetAvailableRoomsByType(RoomType.PatientCare, treatment.TreatmentDuration)
                .ForEach(id => Rooms.Add(new RoomViewModel(roomService.Get(id))));
        }
    }
}