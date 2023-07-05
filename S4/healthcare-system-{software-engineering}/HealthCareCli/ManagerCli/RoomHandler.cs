using HealthCare.Application;
using HealthCare.Application.Common;
using HealthCare.Application.Exceptions;
using HealthCare.Core.Interior;
using HealthCare.WPF.Common;
using HealthCareCli.CliUtil;

namespace HealthCareCli.Manager
{
    public class RoomHandler
    {
        public string RoomHeader => $"{"ID",3} {"NAZIV",15} {"TIP",19}";

        public string RoomToString(Room r)
        {
            return $"{r.Id,3} {r.Name,15} {ViewUtil.Translate(r.Type),19}";
        }

        public Room HandleRoomCreation()
        {
            Console.WriteLine();

            string name = Input.ReadLine("Naziv sobe: ");
            Console.WriteLine("\nTipovi soba");
            RoomType type = HandleRoomTypeChoice();
            
            return new Room(0, name, type);
        }

        public int HandleRoomChoice()
        {
            var roomService = Injector.GetService<RoomService>();

            while (true)
            {
                Console.WriteLine(RoomHeader);
                foreach (var room in roomService.GetAll())
                    Console.WriteLine(RoomToString(room));

                try
                {
                    int id = Input.ReadInt("Id sobe: ", "Nepostojeca opcija.");

                    if (!roomService.Contains(id))
                        throw new ValidationException("Nepostojeca soba.");

                    return id;
                }
                catch (ValidationException ve)
                {
                    Console.WriteLine(ve.Message);
                }
            }
        }

        public RoomType HandleRoomTypeChoice()
        {
            List<RoomType> types = Enum
                .GetValues(typeof(RoomType))
                .Cast<RoomType>().ToList();

            while (true)
            {
                foreach (var (t, i) in Util.WithIndex(types))
                    Console.WriteLine($"{i+1} {ViewUtil.Translate(t)}");
                try
                {
                    int choice = Input.ReadInt("Izbor: ", "Nepostojeca opcija.");
                    if (choice < 0 || choice >= types.Count+1)
                        throw new ValidationException("Nepostojeca opcija.");

                    return types[choice-1];
                }
                catch (ValidationException ve)
                {
                    Console.WriteLine(ve.Message);
                }
            }
        }
    }
}