using HealthCare.Application;
using HealthCare.Application.Exceptions;
using HealthCare.Core.Scheduling;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Scheduling.Schedules;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCareCli.CliUtil;

namespace HealthCareCli.NurseCli
{
    public class AppointmentHandler
    {
        public string GetPatient()
        {
            Console.WriteLine("Izaberite pacijenta");
            PatientService patientService = Injector.GetService<PatientService>();
            List<Patient> patients = patientService.GetAll();

            Presenter.PrintPatientHeader();
            for (int i = 0; i < patients.Count(); i++)
                Presenter.PrintPatient(i + 1, patients[i]);

            int choice = Input.ReadInt("Redni broj pacijenta: ");

            if (choice < 0 || choice - 1 > patients.Count())
                throw new ValidationException("Pogresan unos.");

            return patients[choice - 1].JMBG;
        }

        public List<string> GetSpecialists(string specialization)
        {
            return Injector.GetService<DoctorService>().GetBySpecialization(specialization);
        }


        public string GetSpecialization()
        {
            Console.WriteLine("Izaberite specijalizaciju doktora.");
            DoctorService doctorService = Injector.GetService<DoctorService>();
            List<string> specializations = doctorService.GetSpecializations();

            Presenter.PrintSpecialization(specializations);

            int choice = Input.ReadInt("Redni broj specijalizacije: ");

            if (choice < 0 || choice - 1 > specializations.Count())
                throw new ValidationException("Pogresan unos.");

            return specializations[choice - 1];
        }

        public Appointment GetAppointment(List<string> specialists)
        {
            Console.WriteLine("Izaberite vrstu termina.");
            Console.WriteLine("1. operacija\n2. pregled");
            int choice = Input.ReadInt("Izbor: ");
            TimeSpan span = new TimeSpan(0, 15, 0);

            if (choice != 1 && choice != 2)
                throw new ValidationException("Pogresan unos");

            if (choice == 1)
            {
                Console.WriteLine("Unesite duzinu operacije u minutima.");

                int duration = Input.ReadInt("Duzina trajanja: ");
                span = new TimeSpan(0, duration, 0);
            }

            Appointment appointment = new Appointment();
            appointment.TimeSlot = new TimeSlot(DateTime.Now, span);
            appointment.IsOperation = choice == 1;

            Appointment? urgent = Injector.GetService<Schedule>().TryGetUrgent(span, specialists);

            if (urgent is not null)
            {
                appointment.TimeSlot.Start = urgent.TimeSlot.Start;
                appointment.DoctorJMBG = urgent.DoctorJMBG;
            }

            return appointment;
        }

        public void AddUrgent(Appointment appointment)
        {
            Injector.GetService<Schedule>().AddUrgent(appointment);
        }

        public List<Appointment> GetPostponable(TimeSpan duration, List<string> specialists)
        {
            List<Appointment> postponable = new List<Appointment>();

            foreach (string doctor in specialists)
                postponable.AddRange(Injector.GetService<DoctorSchedule>().GetPostponable(duration, doctor));


            postponable = postponable
                .OrderBy(x => Injector.GetService<Schedule>().GetSoonestTimeSlot(x).Start).ToList();

            return postponable;
        }

        public void Postpone(Appointment newAppointment, List<string> specialists)
        {
            var postponable = GetPostponable(newAppointment.TimeSlot.Duration, specialists);

            if (postponable.Count == 0)
                throw new ValidationException("Nema dostupnih termina za odlaganje.");

            Console.WriteLine("Izaberite termin koji zelite da pomerite.");

            Console.WriteLine($"{"BR",3} {"PACIJENT",20} {"DOKTOR",12} {"VREME I TRAJANJE",30} {"OPERACIJA?",10}");

            for (int i = 0; i < postponable.Count(); i++)
                Presenter.PrintAppointment(i + 1, postponable[i]);

            int choice = Input.ReadInt("Unesite termin: ");

            if (choice < 0 || choice > postponable.Count)
                throw new ValidationException("Pogresan unos.");

            Appointment appointment = postponable[choice - 1];

            newAppointment.DoctorJMBG = appointment.DoctorJMBG;
            newAppointment.TimeSlot.Start = appointment.TimeSlot.Start;
            Injector.GetService<Schedule>().Postpone(appointment);
        }
    }
}