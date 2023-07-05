using HealthCare.Application;
using HealthCare.Core.HumanResources;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.ManagerGUI.HumanResources
{
    public class AbsenceRequestViewModel
    {
        private AbsenceRequest _request;

        public AbsenceRequestViewModel(AbsenceRequest request)
        {
            _request = request;
            Doctor doctor = Injector.GetService<DoctorService>().Get(request.RequesterJMBG);
            EmployeeName = doctor.Name + " " + doctor.LastName;
        }

        public int Id => _request.Id;
        public string EmployeeName { get; }
        public string Reason => _request.Reason;
        public string Start => ViewUtil.ToString(_request.AbsenceDuration.Start, true);
        public string End => ViewUtil.ToString(_request.AbsenceDuration.End, true);
        public string IsApproved => ViewUtil.Translate(_request.IsApproved);
    }
}