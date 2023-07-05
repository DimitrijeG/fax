using System.Windows;
using LibraryCirculation.Core.Finance;
using LibraryCirculation.Core.Fund.Books;
using LibraryCirculation.Core.Renting;
using LibraryCirculation.Core.Users;
using LibraryCirculation.Core.Users.Readers;

namespace LibraryCirculation.WPF.Common
{
    public static class ViewUtil
    {
        public static void ShowInformation(string message)
        {
            MessageBox.Show(message, "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // For user induced errors
        public static void ShowWarning(string message)
        {
            MessageBox.Show(message, "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        // For INTERNAL errors
        public static void ShowError(string message)
        {
            MessageBox.Show(message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static MessageBoxResult ShowConfirmation(string message)
        {
            return MessageBox.Show(message, "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        public static string Translate(bool b)
        {
            return b ? "da" : "ne";
        }

        public static string Translate(Role role)
        {
            return role switch
            {
                Role.Reader => "čitalac",
                Role.Librarian => "bibliotekar",
                Role.ChiefLibrarian => "viši bibliotekar",
                _ => "administrator"
            };
        }

        public static string Translate(BookFormat format)
        {
            return format switch
            {
                BookFormat.Small => "mali",
                BookFormat.Medium => "srednji",
                BookFormat.Large => "veliki",
                BookFormat.EBook => "e-knjiga",
                _ => "audio-knjiga"
            };
        }

        public static string Translate(ReservationStatus status)
        {
            return status switch
            {
                ReservationStatus.Submitted => "na čekanju",
                ReservationStatus.Approved => "prihvaćena",
                _ => "odbijena"
            };
        }

        public static string Translate(MembershipCategory category)
        {
            return category switch
            {
                MembershipCategory.Kid => "deca",
                MembershipCategory.Student => "studenti",
                MembershipCategory.Adult => "adulti",
                _ => "penzioneri"
            };
        }

        public static string Translate(PaymentType type)
        {
            return type switch
            {
                PaymentType.Compensation => "nadoknada",
                PaymentType.Membership => "članarina",
                _ => "kazna"
            };
        }
    }
}