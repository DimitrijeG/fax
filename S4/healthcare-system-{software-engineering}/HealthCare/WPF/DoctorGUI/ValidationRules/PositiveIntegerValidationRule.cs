using System.Globalization;
using System.Windows.Controls;

namespace HealthCare.WPF.DoctorGUI.ValidationRules
{
    public class PositiveIntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse(value.ToString(), out int someResult))
            {
                return someResult > 0
                    ? ValidationResult.ValidResult
                    : new ValidationResult(false, "Broj mora biti veci od nule");
            }

            return new ValidationResult(false, "Nije broj");
        }
    }
}