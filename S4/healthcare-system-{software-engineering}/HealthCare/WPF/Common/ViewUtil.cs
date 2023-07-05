using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using HealthCare.Application.Common;
using HealthCare.Core.Interior;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.Core.PhysicalAssets;
using HealthCare.Core.Users.Model;

namespace HealthCare.WPF.Common
{
    public static class ViewUtil
    {
        public static void ShowInformation(string message)
        {
            MessageBox.Show(message, "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // For user induced errors
        public static void ShowWarning(string message)
        {
            MessageBox.Show(message, "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        // For INTERNAL errors
        public static void ShowError(string message)
        {
            MessageBox.Show(message, "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static MessageBoxResult ShowConfirmation(string message)
        {
            return MessageBox.Show(message, "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        public static string Translate(bool b)
        {
            return b ? "da" : "ne";
        }

        public static string Translate(Gender gender)
        {
            switch (gender)
            {
                case Gender.Female:
                    return "zensko";
                default:
                    return "musko";
            }
        }

        public static string Translate(EquipmentType type)
        {
            switch (type)
            {
                case EquipmentType.Examinational:
                    return "za preglede";
                case EquipmentType.Operational:
                    return "operaciona";
                case EquipmentType.RoomFurniture:
                    return "sobni namestaj";
                default:
                    return "oprema za hodnike";
            }
        }

        public static string Translate(RoomType type)
        {
            switch (type)
            {
                case RoomType.Examinational:
                    return "za preglede";
                case RoomType.Operational:
                    return "operaciona";
                case RoomType.PatientCare:
                    return "smestaj bolesnika";
                case RoomType.Reception:
                    return "recepcija";
                default:
                    return "magacin";
            }
        }

        public static string Translate(MealTime mealTime)
        {
            switch (mealTime)
            {
                case MealTime.AfterMeal:
                    return "posle jela";
                case MealTime.BeforeMeal:
                    return "pre jela";
                case MealTime.DuringMeal:
                    return "tokom jela";
                default:
                    return "bilo kad";
            }
        }

        public static List<string> GetStringList(string text, char delimiter = ',')
        {
            return text.Split(delimiter)
                .Select(x => x.Trim())
                .ToList();
        }

        public static List<int> GetIntList(string text, char delimiter = ',')
        {
            return Array.ConvertAll(
                    text.Split(delimiter).Select(x => x.Trim())
                        .ToArray(), int.Parse)
                .ToList();
        }

        public static string ToString(IEnumerable<string> arr, string delimiter = ", ")
        {
            return string.Join(delimiter, arr);
        }

        public static string ToString(DateTime date, bool shortFormat = false)
        {
            var format = shortFormat ? Formats.SHORTDATETIME : Formats.DATETIME;
            return date.ToString(format);
        }
    }
}