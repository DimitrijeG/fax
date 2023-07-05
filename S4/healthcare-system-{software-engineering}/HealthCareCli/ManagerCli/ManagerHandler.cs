using HealthCare.Application;
using HealthCareCli.CliUtil;
using HealthCareCli.Renovation;

namespace HealthCareCli.Manager
{
    public class ManagerHandler
    {
        public void Show()
        {
            string input;
            while (true)
            {
                Console.WriteLine("============ MENADŽER ============\n");
                Console.WriteLine("1 Složeno renoviranje");
                Console.WriteLine("q Odjava");

                input = Input.ReadLine("\nOpcija: ").ToLower();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        new RenovationHandler().Handle();
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
    }
}