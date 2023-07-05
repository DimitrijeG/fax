using HealthCare.Application;
using HealthCare.Application.Exceptions;
using HealthCare.Core.Interior;
using HealthCare.Core.Interior.Renovation.Model;
using HealthCare.Core.Interior.Renovation.Service;
using HealthCare.Core.Scheduling;
using HealthCare.Core.Scheduling.Schedules;
using HealthCareCli.CliUtil;
using HealthCareCli.Exceptions;
using HealthCareCli.Manager;

namespace HealthCareCli.Renovation
{
    public class RenovationHandler
    {
        private readonly RoomHandler roomHandler = new();
        private readonly RoomSchedule roomSchedule;

        public RenovationHandler()
        {
            roomSchedule = Injector.GetService<RoomSchedule>();
        }

        public void Handle()
        {
            try
            {
                TimeSlot slot;
                while (true)
                {
                    slot = DefaultHandler.HandleTimeSlotChoice();
                    if (slot.Start > DateTime.Now)
                        break;

                    Console.WriteLine("Renovacija ne može da bude u prošlosti.");
                }
                Console.WriteLine();

                bool splitting = GetRenovationType();
                Console.WriteLine();

                if (splitting) HandleSplitting(slot);
                else HandleJoining(slot);
            }
            catch (ValidationException ve)
            {
                Console.WriteLine(ve.Message + "\n");
            }
        }

        private void HandleSplitting(TimeSlot slot)
        {
            Console.WriteLine("Izaberite sobu za deljenje");
            var roomId = roomHandler.HandleRoomChoice();
            if (!roomSchedule.IsAvailable(roomId, slot))
                throw new ValidationException("Izabrana soba nije slobodna u datom terminu.");

            Console.WriteLine("\nUnos prve nove sobe");
            var newRoom1 = roomHandler.HandleRoomCreation();
            Console.WriteLine("\nUnos druge nove sobe");
            var newRoom2 = roomHandler.HandleRoomCreation();

            var renovation = new SplittingRenovation(roomId, slot, newRoom1, newRoom2);
            Injector.GetService<SplittingRenovationService>().Add(renovation);
            Console.WriteLine("\nUspešno zakazano deljenje sobe.\n");
        }

        private void HandleJoining(TimeSlot slot)
        {
            int roomId1, roomId2;

            while (true)
            {
                Console.WriteLine("Izbor prve sobe za spajanje");
                roomId1 = roomHandler.HandleRoomChoice();
                if (!roomSchedule.IsAvailable(roomId1, slot))
                    throw new ValidationException("Izabrana soba nije slobodna u datom terminu.");

                Console.WriteLine("\nIzbor druge sobe za spajanje");
                roomId2 = roomHandler.HandleRoomChoice();
                if (!roomSchedule.IsAvailable(roomId2, slot))
                    throw new ValidationException("Izabrana soba nije slobodna u datom terminu.");

                if (roomId1 != roomId2) break;
                Console.WriteLine("Id-jevi soba ne smeju da budu isti.");
            }

            Console.WriteLine("\nUnos sobe koja će biti kreirana");
            var newRoom = roomHandler.HandleRoomCreation();

            var renovation = new JoiningRenovation(roomId1, slot, roomId2, newRoom);
            Injector.GetService<JoiningRenovationService>().Add(renovation);
            Console.WriteLine("\nUspešno zakazano deljenje sobe.\n");
        }

        private bool GetRenovationType()
        {
            while (true)
            {
                Console.WriteLine("Tip renoviranja");
                Console.WriteLine("1 spajanje");
                Console.WriteLine("2 deljenje\n");

                try
                {
                    var choice = Input.ReadInt("Izbor: ", "Nepostojeća opcija.");
                    if (choice != 1 && choice != 2)
                        throw new ValidationException("Nepostojeća opcija.");

                    return choice == 2;
                }
                catch (ValidationException ve)
                {
                    Console.WriteLine(ve.Message);
                }
            }
        }
    }
}