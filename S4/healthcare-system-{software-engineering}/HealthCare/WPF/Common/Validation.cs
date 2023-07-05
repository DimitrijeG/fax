namespace HealthCare.WPF.Common
{
    public class Validation
    {
        public static bool IsNatural(string number)
        {
            return int.TryParse(number, out var n) && n > 0;
        }
    }
}