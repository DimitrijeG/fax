using System;
using System.Windows;
using HealthCare.Application;
using HealthCare.Core.Notifications;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.PatientGUI.Notifications
{
    public partial class CreateNotificationView
    {
        PatientNotificationsViewModel _previousViewModel;
        UserNotificationService _userNotificationService;

        public CreateNotificationView(PatientNotificationsViewModel previousViewModel)
        {
            _previousViewModel = previousViewModel;
            _userNotificationService = Injector.GetService<UserNotificationService>();
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string caption = txtCaption.Text;
            string text = txtText.Text;
            DateTime selectedDate = datePicker.SelectedDate ?? DateTime.Now.Date;
            int hours, minutes;

            if (string.IsNullOrWhiteSpace(caption) || string.IsNullOrWhiteSpace(text) ||
                !int.TryParse(txtHours.Text, out hours) || !int.TryParse(txtMinutes.Text, out minutes) ||
                datePicker.SelectedDate == null)
            {
                ViewUtil.ShowWarning("Molimo vas unesite sva polja");
                return;
            }

            if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59)
            {
                ViewUtil.ShowWarning("Molimo vas unesite validno vreme");
                return;
            }

            DateTime selectedDateTime =
                new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hours, minutes, 0);
            if (selectedDateTime < DateTime.Now)
            {
                ViewUtil.ShowWarning(
                    "Datum i vreme koje ste uneli su vec prosli, molimo vas unesite drugi datum i vreme");
                return;
            }

            UserNotification notification =
                new UserNotification(Context.Current.JMBG, selectedDateTime, caption, text, true);
            _userNotificationService.Add(notification);
            MessageBox.Show("Uspesno ste dodali notifikaciju");
            _previousViewModel.LoadNotifications();
        }
    }
}