using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HealthCare.Application;
using HealthCare.Core.Communication;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;
using HealthCare.WPF.PatientGUI.Communication.Chats;

namespace HealthCare.WPF.PatientGUI.Communication.Contacts
{
    public class AddContactViewModel
    {
        private readonly ContactService contactService;
        private readonly DoctorService doctorService;
        private readonly NurseService nurseService;


        private ObservableCollection<User> allUsers;

        private User selectedUser;


        public AddContactViewModel(ChatViewModel previousModel)
        {
            nurseService = Injector.GetService<NurseService>();
            doctorService = Injector.GetService<DoctorService>();
            contactService = Injector.GetService<ContactService>();
            fillAllUsers();


            AddContactCommand = new RelayCommand(o =>
            {
                if (selectedUser != null)
                {
                    if (selectedUser.JMBG == Context.Current.JMBG)
                    {
                        ViewUtil.ShowError("Ne mozete dodati sebe");
                        return;
                    }

                    int count = contactService.GetForUser(Context.Current.JMBG).Count(contact =>
                        contact.Participants.Contains(Context.Current.JMBG) &&
                        contact.Participants.Contains(selectedUser.JMBG));

                    if (count > 0)
                    {
                        ViewUtil.ShowError("Vec imate tog kontakta");
                    }
                    else
                    {
                        contactService.Add(
                            new Core.Communication.Contact
                            {
                                Participants = new List<string> { Context.Current.JMBG, SelectedUser.JMBG }
                            }
                        );
                        previousModel.loadContacts();
                        ViewUtil.ShowInformation("Uspesno ste dodali kontakt");
                    }
                }
                else
                {
                    ViewUtil.ShowError("Morate izabrati korisnika");
                }
            });
        }

        public RelayCommand AddContactCommand { get; set; }

        public ObservableCollection<User> AllUsers
        {
            get { return allUsers; }
            set { allUsers = value; }
        }

        public User SelectedUser
        {
            get { return selectedUser; }
            set { selectedUser = value; }
        }

        public void fillAllUsers()
        {
            List<User> AllUsersList = new List<User>();
            foreach (var item in nurseService.GetAll())
            {
                AllUsersList.Add(item);
            }

            foreach (var item in doctorService.GetAll())
            {
                AllUsersList.Add(item);
            }

            AllUsers = new ObservableCollection<User>(AllUsersList);
        }
    }
}