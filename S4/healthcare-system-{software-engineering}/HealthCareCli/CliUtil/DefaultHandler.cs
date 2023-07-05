using HealthCare.Application.Exceptions;
using HealthCare.Core.Scheduling;

namespace HealthCareCli.CliUtil
{
    public static class DefaultHandler
    {
        public static TimeSlot HandleTimeSlotChoice()
        {
            while (true)
            {
                DateTime start = HandleDateTimeChoice("Početni datum: ");
                DateTime end = HandleDateTimeChoice("Krajnji datum: ");
                if (start < end)
                    return new TimeSlot(start, end);

                Console.WriteLine("Krajnji datum mora da bude pre početnog");
            }
        }

        public static DateTime HandleDateTimeChoice(string prompt)
        {
            while (true)
            {
                try
                {
                    return Input.ReadDate(prompt);
                }
                catch (ValidationException ve)
                {
                    Console.WriteLine(ve.Message);
                }
            }
        }

        public static int HandleIntRanged(string prompt, int lowerBound, int upperBound)
        {
            while (true)
            {
                try
                {
                    int input = Input.ReadInt(prompt);
                    if (input >= lowerBound && input <= upperBound)
                    {
                        return input;
                    }

                    Console.WriteLine($"Unos mora biti izmedju {lowerBound} i {upperBound}");
                }
                catch (ValidationException ve)
                {
                    Console.WriteLine(ve.Message);
                }
            }
        }

        public static int HandleInt(string prompt)
        {
            while (true)
            {
                try
                {
                    int input = Input.ReadInt(prompt);
                    return input;
                }
                catch (ValidationException ve)
                {
                    Console.WriteLine(ve.Message);
                }
            }
        }

        public static bool TryParseTime(string timeInput, out int hours, out int minutes)
        {
            hours = 0;
            minutes = 0;

            if (string.IsNullOrWhiteSpace(timeInput))
            {
                return false;
            }

            string[] parts = timeInput.Split(':');
            if (parts.Length != 2 || !int.TryParse(parts[0], out hours) || !int.TryParse(parts[1], out minutes))
            {
                return false;
            }

            if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59)
            {
                return false;
            }

            return true;
        }
    }
}