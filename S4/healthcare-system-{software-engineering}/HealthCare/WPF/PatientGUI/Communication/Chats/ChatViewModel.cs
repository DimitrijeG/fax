using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using HealthCare.Application;
using HealthCare.Core.Communication;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;
using HealthCare.WPF.PatientGUI.Communication.Contacts;

namespace HealthCare.WPF.PatientGUI.Communication.Chats
{
    public class ChatViewModel : ViewModelBase
    {
        public readonly DoctorService doctorService;

        public readonly MessageService messageService;

        public readonly NurseService nurseService;


        private string _message;

        private ContactViewModel _selectedContact;
        private ObservableCollection<ContactViewModel> contacts;


        public ContactService contactService;

        private SolidColorBrush loggedColorBrush;

        private User loggedUser;

        private string otherUsername;

        public ChatViewModel()
        {
            Contacts = new ObservableCollection<ContactViewModel>();
            doctorService = Injector.GetService<DoctorService>();
            contactService = Injector.GetService<ContactService>();
            messageService = Injector.GetService<MessageService>();
            nurseService = Injector.GetService<NurseService>();
            LoggedUser = Context.Current;
            loadContacts();

            SendCommand = new RelayCommand(o =>
            {
                if (_selectedContact != null)
                {
                    Message message = new Message()
                    {
                        contactID = _selectedContact.contact.ID,
                        MessageText = Message,
                        SenderJMBG = Context.Current.JMBG,
                        Time = DateTime.Now,
                        Seen = false
                    };
                    MessageViewModel messageViewModel = new MessageViewModel(message);

                    if (SelectedContact.Messages.Count > 0)
                    {
                        Message lastMessage = SelectedContact.Messages.Last()._Message;
                        if (message.SenderJMBG == lastMessage.SenderJMBG &&
                            isSameMinute(message.Time, lastMessage.Time))
                        {
                            messageViewModel.IsFirst = false;
                        }
                        else
                        {
                            messageViewModel.IsFirst = true;
                        }
                    }
                    else
                    {
                        messageViewModel.IsFirst = true;
                    }


                    _selectedContact.Messages.Add(messageViewModel);
                    messageService.Add(message);
                }

                Message = "";
            });

            OpenAddContactWindow = new RelayCommand(o =>
            {
                AddContactView addContactWindow = new AddContactView(this);
                addContactWindow.ShowDialog();
            });
        }


        public ObservableCollection<ContactViewModel> Contacts
        {
            get { return contacts; }
            set
            {
                contacts = value;
                OnPropertyChanged(nameof(Contacts));
            }
        }

        public User LoggedUser
        {
            get => loggedUser;
            set
            {
                {
                    loggedUser = value;
                    OnPropertyChanged();
                    LoggedColorBrush = CalculateLoggedColor();
                }
            }
        }

        public SolidColorBrush LoggedColorBrush
        {
            get { return loggedColorBrush; }
            set
            {
                loggedColorBrush = value;
                OnPropertyChanged(nameof(LoggedColorBrush));
            }
        }

        public RelayCommand SendCommand { get; set; }
        public RelayCommand OpenAddContactWindow { get; set; }

        public ContactViewModel SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                _selectedContact = value;
                if (value is null) return;
                foreach (MessageViewModel message in _selectedContact.Messages.Where(x =>
                             x._Message.SenderJMBG != Context.Current.JMBG))
                {
                    message._Message.Seen = true;
                    messageService.Update(message._Message);
                }

                _selectedContact.RecalculateAll();
                OtherUsername = _selectedContact.OtherUsername;
                OnPropertyChanged(nameof(SelectedContact));
            }
        }

        public string OtherUsername
        {
            get { return otherUsername; }
            set
            {
                otherUsername = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler ScrollToBottom;


        public void loadContacts()
        {
            List<Contact> contacts = contactService.GetForUser(Context.Current.JMBG);
            List<ContactViewModel> contactViewModels = new List<ContactViewModel>();
            foreach (Contact contact in contacts)
            {
                ContactViewModel a = new ContactViewModel(contact);
                contactViewModels.Add(a);
            }

            Contacts = new ObservableCollection<ContactViewModel>(contactViewModels);
        }

        private SolidColorBrush CalculateLoggedColor()
        {
            string loggedColor = Context.Current.Color ?? "#FF0000";
            Color color = (Color)ColorConverter.ConvertFromString(loggedColor);
            SolidColorBrush brush = new SolidColorBrush(color);
            return brush;
        }


        public bool isSameMinute(DateTime message1Timestamp, DateTime message2Timestamp)
        {
            return message1Timestamp.Year == message2Timestamp.Year &&
                   message1Timestamp.Month == message2Timestamp.Month &&
                   message1Timestamp.Day == message2Timestamp.Day &&
                   message1Timestamp.Hour == message2Timestamp.Hour &&
                   message1Timestamp.Minute == message2Timestamp.Minute;
        }
    }
}