using NaijaConnect.Models;
using NaijaConnect.Services;
using NaijaConnect.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NaijaConnect.ViewModels
{
    public class NewChatPageViewModel : ViewModelBase
    {
        RemoteService remoteService;
        //DataService dataService;

        private DelegateCommand DelegateSelectionCommand;
        public DelegateCommand NavigateSelectionCommand => DelegateSelectionCommand ?? (DelegateSelectionCommand = new DelegateCommand(ExecuteSelectionCommand));

        

        //public ICommand searchCommand => new Command<string>(LoadUser);

        public ObservableCollection<User> AllUsers { get; set; }

        public User userSelection;

        public User UserSelection
        {
            get
            {
                return userSelection;
            }
            set
            {
                SetProperty(ref userSelection, value);
            }
        }


        //public ObservableCollection<NewChats> newContacts;
        
        //public ObservableCollection<NewChats> NewContacts
        //{
        //    get
        //    {
        //        return newContacts;
        //    }
        //    set
        //    {
        //        SetProperty(ref newContacts, value);
        //    }
        //}

        public NewChatPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            //dataService = new DataService();
            remoteService = new RemoteService();
            AllUsers = new ObservableCollection<User>();
            GetAllUsers();
        }

        private async void GetAllUsers()
        {
            try
            {
                var users = await remoteService.GetAllUser();
                AllUsers.Clear();
                foreach (var item in users)
                {
                    if (item.Id != Settings.KeySettings)
                    {
                        AllUsers.Add(item);
                    }

                }
            }
            catch
            {

            }
        }

        private async void ExecuteSelectionCommand()
        {
            if (UserSelection != null)
            {
                var parameters = new NavigationParameters();
                parameters.Add("nav", UserSelection);
                await NavigationService.NavigateAsync("ChatPage", parameters);
                UserSelection = null;
            }
        }

        //public void LoadUser(string query)
        //{
        //    Task.Run(async () =>
        //    {
        //        NewContacts = await dataService.GetUsersAsync(query);

        //    });
        //}


    }
}

