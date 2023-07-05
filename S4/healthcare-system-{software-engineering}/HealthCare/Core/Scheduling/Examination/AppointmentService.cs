using System.Collections.Generic;
using System.Linq;
using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.Scheduling.Examination
{
    public class AppointmentService : NumericService<Appointment>
    {
        public AppointmentService(IRepository<Appointment> repository) : base(repository)
        {
        }

        public List<Appointment> GetByDoctor(string doctorJMBG)
        {
            return GetAll().Where(x => x.DoctorJMBG == doctorJMBG).ToList();
        }

        public List<Appointment> GetByPatient(string patientJMBG)
        {
            return GetAll().Where(x => x.PatientJMBG == patientJMBG).ToList();
        }

        public List<Appointment> GetOverlapping(Appointment appointment)
        {
            return GetAll().FindAll(x =>
                x.AppointmentID != appointment.AppointmentID &&
                x.TimeSlot.Overlaps(appointment.TimeSlot) && (
                    x.PatientJMBG.Equals(appointment.PatientJMBG) ||
                    x.DoctorJMBG.Equals(appointment.DoctorJMBG) ||
                    x.RoomID.Equals(appointment.RoomID)));
        }

        public List<string> GetExaminedPatients(string doctorJMBG)
        {
            return GetAll()
                .Where(x => x.DoctorJMBG == doctorJMBG)
                .Select(x => x.PatientJMBG)
                .Distinct().ToList();
        }
    }
}