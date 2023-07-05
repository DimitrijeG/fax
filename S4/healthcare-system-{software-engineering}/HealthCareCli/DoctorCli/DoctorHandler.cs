using HealthCare.Application;
using HealthCare.Application.Common;
using HealthCare.Application.Exceptions;
using HealthCare.Core.Scheduling;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Users.Model;
using HealthCareCli.CliUtil;

namespace HealthCareCli.DoctorCli
{
    public class DoctorHandler
    {
        private static readonly AppointmentHandler _appointmentHandler = new AppointmentHandler();

        public void Show()
        {
            string input;
            while (true)
            {
                Console.WriteLine("============ Doktor ============\n");
                Console.WriteLine("1 Dodaj pregled/operaciju ");
                Console.WriteLine("2 Ukloni pregled/operaciju");
                Console.WriteLine("3 Ispisi preglede/operacije");
                Console.WriteLine("4 Izmeni pregled/operaciju");
                Console.WriteLine("q Odjava");

                input = Input.ReadLine("\nOpcija: ").ToLower();

                switch (input)
                {
                    case "1":
                        HandleCreateAppointment();
                        break;
                    case "2":
                        HandleDeleteAppointment();
                        break;
                    case "3":
                        HandleReadAppointment();
                        break;
                    case "4":
                        HandleUpdateAppointment();
                        break;
                    case "q":
                        Context.Reset();
                        Console.WriteLine("\n\n");
                        return;
                    default:
                        Console.WriteLine("Nepostojeća opcija. Pokušajte ponovo.\n");
                        break;
                }
            }
        }

        private void HandleCreateAppointment()
        {
            string patientJMBG;
            bool isOperation;
            TimeSpan duration = new TimeSpan(0, 15, 0);
            DateTime date;
            try
            {
                patientJMBG = GetPatientJMBG();
                isOperation = GetIsOperation();
                date = GetDateTime();
                if (isOperation)
                {
                    duration = GetDuration();
                }
            }
            catch (ValidationException ve)
            {
                Console.WriteLine(ve.Message);
                return;
            }

            Appointment appointment = new Appointment(patientJMBG, Context.Current.JMBG, new TimeSlot(date, duration),
                isOperation);
            TrySave(appointment);
        }


        private void HandleReadAppointment()
        {
            Console.WriteLine(
                $"{"ID",4} {"Ime pacijenta",15} {"Prezime pacijenta",20} {"Id sobe",6} {"Operacija?",10} {"Vreme (datum  satnica|trajanje)",40}");
            foreach (Appointment a in _appointmentHandler.GetDoctorAppointments())
            {
                Patient p = _appointmentHandler.GetPatientFromAppointment(a.PatientJMBG);
                Console.WriteLine(
                    $"{a.AppointmentID,4} {p.Name,15} {p.LastName,20} {a.RoomID,6} {a.IsOperation,10} {a.TimeSlot,40}");
            }
        }

        private void HandleUpdateAppointment()
        {
            int appointmentID;
            while (true)
            {
                appointmentID = DefaultHandler.HandleInt("Unesite ID Appointmenta koji zelite da izmenite: ");
                if (_appointmentHandler.AppointmentExist(appointmentID))
                {
                    break;
                }

                Console.WriteLine("Ne postoji pregled/operacija sa unetim ID-em");
            }

            Appointment appointment = _appointmentHandler.GetAppointment(appointmentID);

            string input;
            while (true)
            {
                Console.WriteLine("Odaberite sta zelite da izmenite\n");
                Console.WriteLine("1 Izmeni pacijenta ");
                Console.WriteLine("2 Izmeni datum i vreme");
                Console.WriteLine("3 Izmeni trajanje");
                Console.WriteLine("q Nazad");

                input = Input.ReadLine("\nOpcija: ").ToLower();

                switch (input)
                {
                    case "1":
                        EditAppointmentPatient(appointment);
                        break;
                    case "2":
                        EditAppointmentDateTime(appointment);
                        break;
                    case "3":
                        EditAppointmentDuration(appointment);
                        break;
                    case "q":
                        Console.WriteLine("\n\n");
                        return;
                    default:
                        Console.WriteLine("Nepostojeća opcija. Pokušajte ponovo.\n");
                        break;
                }
            }
        }

        private void EditAppointmentDuration(Appointment appointment)
        {
            if (appointment.IsOperation)
            {
                TimeSpan duration = GetDuration();
                appointment.TimeSlot.Duration = duration;
                TrySave(appointment, false);
            }
            else
            {
                Console.Write("Trajanje se ne moze izmeniti kod pregleda");
            }
        }

        private void EditAppointmentDateTime(Appointment appointment)
        {
            DateTime newDate = GetDateTime();
            appointment.TimeSlot.Start = newDate;
            TrySave(appointment, false);
        }

        private void EditAppointmentPatient(Appointment appointment)
        {
            string patientJmbg = GetPatientJMBG();
            appointment.PatientJMBG = patientJmbg;
            TrySave(appointment, false);
        }

        private void HandleDeleteAppointment()
        {
            int input;
            try
            {
                input = Input.ReadInt("Unesite ID pregleda/operacije koje zelite da uklonite:");
            }
            catch (ValidationException ve)
            {
                Console.WriteLine(ve.Message);
                return;
            }

            if (!_appointmentHandler.DeleteAppointment(input))
            {
                Console.WriteLine($"Pregled/Operacija sa ID-om {input} ne postoji!");
                return;
            }

            Console.WriteLine($"Pregled/Operacija sa ID-om {input} uspesno obrisana!");
        }

        private void PrintAllPatients()
        {
            Console.WriteLine($"{"JMBG pacijenta",15} {"Ime pacijenta",15} {"Prezime pacijenta",20}");
            foreach (var patient in _appointmentHandler.GetAllPatients())
            {
                Console.WriteLine($"{patient.JMBG,15} {patient.Name,15} {patient.LastName,20}");
            }
        }

        private string GetPatientJMBG()
        {
            PrintAllPatients();
            string patientJMBG;
            while (true)
            {
                patientJMBG = Input.ReadLine("Unesite JMBG pacijenta: ");
                if (_appointmentHandler.PatientExist(patientJMBG))
                {
                    break;
                }

                Console.WriteLine("Pacijent nije pronadjen u sistemu!");
            }

            return patientJMBG;
        }

        private bool GetIsOperation()
        {
            while (true)
            {
                string input = Input.ReadLine("Da li je u pitanju operacija (da|ne): ").ToLower();
                switch (input)
                {
                    case "da":
                        return true;
                    case "ne":
                        return false;
                    default:
                        Console.WriteLine("Morate uneti da ili ne");
                        break;
                }
            }
        }

        private DateTime GetDateTime()
        {
            while (true)
            {
                try
                {
                    DateTime date =
                        DefaultHandler.HandleDateTimeChoice($"Unesite datum u formatu {Formats.SHORTDATETIME} :");
                    int hours = DefaultHandler.HandleIntRanged("Unesite u koliko sati (00-23): ", 0, 23);
                    int minutes = DefaultHandler.HandleIntRanged("Unesite u koliko minuta (0-59): ", 0, 59);
                    date = date.AddMinutes(minutes).AddHours(hours);
                    return date;
                }
                catch (ValidationException ve)
                {
                    Console.WriteLine(ve.Message);
                }
            }
        }

        private TimeSpan GetDuration()
        {
            int minutes = DefaultHandler.HandleInt("Unesite trajanje u minutima: ");
            return new TimeSpan(0, minutes, 0);
        }

        private static void TrySave(Appointment appointment, bool isNew = true)
        {
            if (isNew)
            {
                if (!_appointmentHandler.AddAppointment(appointment))
                {
                    Console.WriteLine("Pregled/Operacija nije uspesno sacuvana, termin nije slobodan!");
                    return;
                }

                Console.WriteLine("Pregled/Operacija uspeno sacuvana!");
                return;
            }

            if (!_appointmentHandler.UpdateAppointment(appointment))
            {
                Console.WriteLine("Pregled/Operacija nije uspesno sacuvana, termin nije slobodan!");
                return;
            }

            Console.WriteLine("Pregled/Operacija uspeno sacuvana!");
        }
    }
}