using System.ComponentModel;
using System.Windows.Media;
using HealthCare.Application;
using HealthCare.Core.Communication;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;

namespace HealthCare.WPF.PatientGUI.Communication.Chats
{
    public class MessageViewModel : INotifyPropertyChanged
    {
        private bool isFirst;

        private Message message;

        private string messageText;

        private SolidColorBrush senderColor;

        private string senderName;

        private string time;

        public MessageViewModel(Message messageInput)
        {
            _Message = messageInput;
        }

        public DoctorService doctorService => Injector.GetService<DoctorService>();

        public NurseService nurseService => Injector.GetService<NurseService>();

        public Message _Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged(nameof(_Message));
                recalculateAll();
            }
        }

        public bool IsFirst
        {
            get { return isFirst; }
            set
            {
                isFirst = value;
                OnPropertyChanged(nameof(IsFirst));
            }
        }

        public SolidColorBrush SenderColor
        {
            get { return senderColor; }
            set
            {
                senderColor = value;
                OnPropertyChanged(nameof(SenderColor));
            }
        }

        public string Time
        {
            get { return time; }
            set
            {
                time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        public string SenderName
        {
            get { return senderName; }
            set
            {
                senderName = value;
                OnPropertyChanged(nameof(SenderName));
            }
        }

        public string MessageText
        {
            get { return messageText; }
            set
            {
                messageText = value;
                OnPropertyChanged(nameof(MessageText));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void recalculateAll()
        {
            MessageText = message.MessageText;
            Time = message.Time.ToString();

            User senderUser = doctorService.TryGet(message.SenderJMBG);
            if (senderUser == null)
            {
                senderUser = nurseService.TryGet(message.SenderJMBG);
            }

            SenderName = senderUser.Username;


            User userSender = doctorService.TryGet(message.SenderJMBG);
            if (userSender == null)
            {
                userSender = nurseService.TryGet(message.SenderJMBG);
            }

            string userColor = userSender.Color;

            Color color = (Color)ColorConverter.ConvertFromString(userColor);
            SolidColorBrush brush = new SolidColorBrush(color);
            SenderColor = brush;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}