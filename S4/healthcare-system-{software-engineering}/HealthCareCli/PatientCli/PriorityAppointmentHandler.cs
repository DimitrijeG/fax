using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Users.Model;
using HealthCare.WPF.PatientGUI.Scheduling;
using HealthCareCli.CliUtil;

namespace HealthCareCli.PatientCli
{
    public class PriorityAppointmentHandler
    {
        CreatePriorityAppointmentViewModel viewModel = new CreatePriorityAppointmentViewModel();

        public void Show()
        {
            DateTime date;
            int startHours, startMinutes, endHours, endMinutes;
            string priority;

            while (true)
            {
                while (true)
                {
                    date = DefaultHandler.HandleDateTimeChoice("Unesite krajnji datum(DD - MM - YYYY): ");
                    if (date >= DateTime.Now) break;
                    Console.WriteLine("Datum koji ste uneli je u proslosti, pokusajte ponovo.");
                }


                while (true)
                {
                    Console.Write("Unesite pocetno vreme (HH:MM): ");
                    string startTimeInput = Console.ReadLine();
                    if (DefaultHandler.TryParseTime(startTimeInput, out startHours, out startMinutes))
                    {
                        break;
                    }

                    Console.WriteLine("Neispravan format vremena. Molimo Vas pokusajte ponovo");
                }

                while (true)
                {
                    Console.Write("Unesite krajnje vreme (HH:MM): ");
                    string endTimeInput = Console.ReadLine();
                    if (DefaultHandler.TryParseTime(endTimeInput, out endHours, out endMinutes))
                    {
                        break;
                    }

                    Console.WriteLine("Neispravan format vremena. Molimo Vas pokusajte ponovo");
                }


                while (true)
                {
                    Console.WriteLine(
                        "\nIzaberite doktora (Upisite rednji broj koji se nalazi ispred zeljenog doktora)");
                    int counter = 0;
                    List<Doctor> doctors = viewModel.Doctors;
                    foreach (Doctor doctor in doctors)
                    {
                        Console.WriteLine(counter + ". " + doctor.Name + " " + doctor.LastName + " Specijalizacija: " +
                                          doctor.Specialization + " Prosecna ocena: " + doctor.Rating);
                        counter++;
                    }

                    int selectedDoctorIndex;
                    if (!int.TryParse(Console.ReadLine(), out selectedDoctorIndex))
                    {
                        Console.WriteLine("Neispravan broj");
                        continue;
                    }

                    if (selectedDoctorIndex >= doctors.Count)
                    {
                        Console.WriteLine("Neispravan broj");
                        continue;
                    }

                    viewModel.SelectedDoctor = doctors[selectedDoctorIndex];
                    break;
                }


                while (true)
                {
                    Console.Write("\nUnesite rednji broj prioriteta:\n1.Doktor\n2.Datum\n");
                    string inputPriority = Console.ReadLine();
                    if (inputPriority == "1")
                    {
                        priority = "Doctor";
                        break;
                    }

                    if (inputPriority == "2")
                    {
                        priority = "Date";
                        break;
                    }

                    Console.WriteLine("Neispravan unos.");
                }

                viewModel.EndDate = date;
                viewModel.HourStart = startHours;
                viewModel.MinuteStart = startMinutes;
                viewModel.HourEnd = endHours;
                viewModel.MinuteEnd = endMinutes;
                if (priority == "Doctor") viewModel.IsDoctorPriority = true;
                else viewModel.IsDoctorPriority = false;

                if (!viewModel.ValidateAllData())
                {
                    Console.WriteLine("Nesto od podataka niste uneli dobro");
                    continue;
                }

                viewModel.CalculateAppointments();

                while (true)
                {
                    Console.WriteLine(
                        "\nIzaberite pregled (Upisite rednji broj koji se nalazi ispred zeljenog doktora)");
                    int counter = 0;
                    List<Appointment> appointments = viewModel.ResultAppointments.ToList();
                    foreach (Appointment appointment in appointments)
                    {
                        Console.WriteLine(appointment.AppointmentID + ". Datum:" +
                                          appointment.TimeSlot.Start.ToString() + " Trajanje " +
                                          appointment.TimeSlot.Duration.ToString());
                        counter++;
                    }

                    int selectedAppointmendIndex;
                    if (!int.TryParse(Console.ReadLine(), out selectedAppointmendIndex))
                    {
                        Console.WriteLine("Neispravan broj");
                        continue;
                    }

                    if (selectedAppointmendIndex >= appointments.Count)
                    {
                        Console.WriteLine("Neispravan broj");
                        continue;
                    }

                    viewModel.SelectedAppointment = appointments[selectedAppointmendIndex];
                    viewModel.CreateAppointment();
                    Console.WriteLine("Uspesno kreiran pregled");
                    break;
                }

                break;
            }
        }
    }
}