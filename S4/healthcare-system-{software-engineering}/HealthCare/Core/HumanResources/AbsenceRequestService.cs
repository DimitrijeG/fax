using System.Collections.Generic;
using System.Linq;
using HealthCare.Application;
using HealthCare.Core.Notifications;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.HumanResources
{
    public class AbsenceRequestService : NumericService<AbsenceRequest>
    {
        public AbsenceRequestService(IRepository<AbsenceRequest> repository) : base(repository)
        {
        }

        public IEnumerable<AbsenceRequest> GetDoctorRequests(string doctorJMBG)
        {
            return GetAll().Where(x => x.RequesterJMBG == doctorJMBG);
        }

        public bool OverlappingOther(AbsenceRequest request)
        {
            return GetAll().Any(r =>
                r.Id != request.Id && r.IsApproved &&
                r.RequesterJMBG == request.RequesterJMBG &&
                r.AbsenceDuration.Overlaps(request.AbsenceDuration));
        }

        public void ManageRequest(AbsenceRequest request)
        {
            var appointmentService = Injector.GetService<AppointmentService>();
            var notificationService = Injector.GetService<NotificationService>();

            Update(request);
            if (request.IsApproved)
            {
                appointmentService
                    .GetByDoctor(request.RequesterJMBG)
                    .Where(a => a.TimeSlot.Overlaps(request.AbsenceDuration)).ToList()
                    .ForEach(a =>
                    {
                        appointmentService.Remove(a.AppointmentID);
                        notificationService.Add(new Notification(
                            $"Termin sa id-jem {a.AppointmentID} je otkazan zbog odsustva doktora.",
                            a.PatientJMBG));
                    });
            }

            var a = GetAll().Where(r => !r.IsApproved &&
                                        r.RequesterJMBG == request.RequesterJMBG &&
                                        r.AbsenceDuration.Overlaps(request.AbsenceDuration)).ToList();

            GetAll().Where(r => !r.IsApproved &&
                                r.RequesterJMBG == request.RequesterJMBG &&
                                r.AbsenceDuration.Overlaps(request.AbsenceDuration)).ToList()
                .ForEach(r => Remove(r.Id));
        }
    }
}