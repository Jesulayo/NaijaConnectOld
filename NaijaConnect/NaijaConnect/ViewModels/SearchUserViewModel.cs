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

namespace NaijaConnect.ViewModels
{
    public class SearchUserViewModel : ViewModelBase
    {
        RemoteService remoteService;
        public ObservableCollection<User> MyProperty { get; set; }
        public SearchUserViewModel(INavigationService navigationService)
            :base(navigationService)
        {
            Title = "Users";
            remoteService = new RemoteService();
            MyProperty = new ObservableCollection<User>();
            GetAllUsers();
        }

        private async void GetAllUsers()
        {
            try
            {
                var users = await remoteService.GetAllUser();
                MyProperty.Clear();
                foreach (var item in users)
                {
                    if(item.Id != Settings.KeySettings)
                    {
                        MyProperty.Add(item);
                    }

                }
            }
            catch
            {

            }
        }
    }
}
