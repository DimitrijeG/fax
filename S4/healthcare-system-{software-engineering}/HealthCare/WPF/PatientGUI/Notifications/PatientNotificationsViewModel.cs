using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using HealthCare.Application;
using HealthCare.Core.Notifications;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.Core.Users.Model;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.PatientGUI.Notifications
{
    public class PatientNotificationsViewModel : INotifyPropertyChanged
    {
        private readonly MedicationService _medicationService;
        private readonly PrescriptionService _prescriptionService;
        private readonly UserNotificationService _userNotificationService;
        private ObservableCollection<UserControl> _notificationControls;
        private ObservableCollection<UserNotification> _notifications;
        private List<Prescription> prescriptions;

        public PatientNotificationsViewModel()
        {
            _userNotificationService = Injector.GetService<UserNotificationService>();
            _prescriptionService = Injector.GetService<PrescriptionService>(Injector.REGULAR_PRESCRIPTION_S);
            _medicationService = Injector.GetService<MedicationService>();
            Notifications = new ObservableCollection<UserNotification>();
            NotificationControls = new ObservableCollection<UserControl>();
            LoadNotifications();
        }

        public ObservableCollection<UserNotification> Notifications
        {
            get { return _notifications; }
            set
            {
                _notifications = value;
                OnPropertyChanged(nameof(Notifications));
            }
        }

        public ObservableCollection<UserControl> NotificationControls
        {
            get { return _notificationControls; }
            set
            {
                _notificationControls = value;
                OnPropertyChanged(nameof(NotificationControls));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void LoadNotifications()
        {
            GetNotifications();
            GetNotificationControls();
        }

        public void GetNotifications()
        {
            string currentUserJMBG = Context.Current.JMBG;
            List<UserNotification> userNotifications = _userNotificationService.GetForUser(currentUserJMBG);
            DateTime currentTime = DateTime.Now;
            prescriptions = _prescriptionService.GetPatientsPrescriptions(currentUserJMBG);
            foreach (Prescription prescription in prescriptions)
            {
                foreach (DateTime pillDateTime in prescription.GetPillConsumptionTimes())
                {
                    if (pillDateTime > currentTime)
                    {
                        Medication medication = _medicationService.Get(prescription.MedicationId);
                        string notificationMessage = "Lek: " + medication.Name + "\n"
                                                     + "Instrukcije: " + ViewUtil.Translate(prescription.Instruction) +
                                                     "\n"
                                                     + "Vreme uzimanja leka: " + pillDateTime.ToString();
                        userNotifications.Add(new UserNotification(currentUserJMBG, pillDateTime, "Popijte tabletu",
                            notificationMessage, false));
                    }
                }
            }

            Patient patient = (Patient)Context.Current;
            userNotifications = userNotifications
                .Where(notification =>
                    (notification.receiveTime - currentTime) < TimeSpan.FromHours(patient.NotificationHours))
                .OrderBy(notification => notification.receiveTime)
                .ToList();
            Notifications.Clear();
            foreach (var notification in userNotifications)
            {
                Notifications.Add(notification);
            }
        }

        public void GetNotificationControls()
        {
            NotificationControls.Clear();
            foreach (var notification in Notifications)
            {
                var notificationControl = new NotificationControl(notification);
                NotificationControls.Add(notificationControl);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}