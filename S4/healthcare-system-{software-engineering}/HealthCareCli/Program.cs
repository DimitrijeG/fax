using HealthCare.Application;
using HealthCare.Application.Exceptions;
using HealthCare.Core.Users.Service;
using HealthCareCli.CliUtil;
using HealthCareCli.DoctorCli;
using HealthCareCli.Manager;
using HealthCareCli.Nurse;
using HealthCareCli.PatientCli;

namespace HealthCareCli
{
    public class Program
    {
        public static void Main()
        {
            string input;
            while (true)
            {
                Console.WriteLine("============ OPCIJE ============\n");
                Console.WriteLine("1 Prijava");
                Console.WriteLine("q Exit");

                input = Input.ReadLine("\nOpcija: ").ToLower();

                switch (input)
                {
                    case "1":
                        Role? loginRole = LoginHandler();
                        if (loginRole != null)
                            MenuHandler(loginRole);
                        break;
                    case "q":
                        return;
                    default:
                        Console.WriteLine("Nepostojeća opcija. Pokušajte ponovo.\n");
                        break;
                }
            }
        }

        private static Role? LoginHandler()
        {
            string username, password;

            username = Input.ReadLine("\nKorisničko ime: ");
            password = Input.ReadLine("Lozinka: ");
            Console.WriteLine();

            try
            {
                return Injector.GetService<LoginService>().Login(username, password);
            }
            catch (LoginException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private static void MenuHandler(Role? loginRole)
        {
            switch (loginRole)
            {
                case Role.Nurse:
                    new NurseHandler().Show();
                    break;
                case Role.Patient:
                    new PatientHandler().Show();
                    break;
                case Role.Manager:
                    new ManagerHandler().Show();
                    break;
                case Role.Doctor:
                    new DoctorHandler().Show();
                    break;
                default:
                    Console.WriteLine("Nije implementirano.");
                    Context.Reset();
                    break;
            }
        }
    }
}