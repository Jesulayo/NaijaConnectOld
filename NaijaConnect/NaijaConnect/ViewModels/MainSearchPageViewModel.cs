using NaijaConnect.Models;
using NaijaConnect.Services;
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
    public class MainSearchPageViewModel : ViewModelBase
    {
        DataService dataService;

        public ICommand searchCommand => new Command<string>(LoadUser);


        public ObservableCollection<NewChats> newContacts;
        public ObservableCollection<NewChats> NewContacts
        {
            get
            {
                return newContacts;
            }
            set
            {
                SetProperty(ref newContacts, value);
            }
        }

        public MainSearchPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            dataService = new DataService();
        }

        public void LoadUser(string query)
        {
            Task.Run(async () =>
            {
                NewContacts = await dataService.GetUsersAsync(query);

            });
        }
    }
}
