using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HealthCare.Application;
using HealthCare.Core.HumanResources;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.DoctorGUI.AbsenceRequesting
{
    public class AbsenceRequestViewModel : ViewModelBase
    {
        private readonly AbsenceRequestService _absenceRequestService;
        private ObservableCollection<AbsenceRequestDTO> _doctorRequests;

        public AbsenceRequestViewModel()
        {
            _absenceRequestService = Injector.GetService<AbsenceRequestService>();
            _doctorRequests = new ObservableCollection<AbsenceRequestDTO>();
            AbsenceStartingDate = DateTime.Today.AddDays(2);
            AbsenceReason = "";
            MakeAbsenceRequestCommand = new MakeAbsenceRequestCommand(this);

            Update();
        }

        public IEnumerable<AbsenceRequestDTO> DoctorRequests => _doctorRequests;
        public string AbsenceReason { get; set; }
        public int AbsenceDurationDays { get; set; }
        public DateTime AbsenceStartingDate { get; set; }
        public ICommand MakeAbsenceRequestCommand { get; }

        public void Update()
        {
            _doctorRequests.Clear();
            foreach (var request in _absenceRequestService.GetDoctorRequests(Context.Current.JMBG))
            {
                _doctorRequests.Add(new AbsenceRequestDTO(request));
            }
        }
    }
}