using NaijaConnect.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NaijaConnect.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private DelegateCommand ProfileCommand;
        private DelegateCommand SearchCommand;

        public DelegateCommand NavigateProfileCommand => ProfileCommand ?? (ProfileCommand = new DelegateCommand(ExecuteNavigateToProfileCommand));
        public DelegateCommand NavigateSearchCommand => SearchCommand ?? (SearchCommand = new DelegateCommand(ExecuteNavigateToSearchCommand));

        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                SetProperty(ref username, value);
            }
        }

        private string avatar;
        public string Avatar
        {
            get
            {
                return avatar;
            }
            set
            {
                SetProperty(ref avatar, value);
            }
        }

        public HomePageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Username = Settings.UsernameSettings;
            Avatar = Settings.AvatarSettings;
        }

        private void ExecuteNavigateToProfileCommand()
        {
            NavigationService.NavigateAsync("ProfilePage");
        }

        private void ExecuteNavigateToSearchCommand()
        {
            NavigationService.NavigateAsync("SearchPage");
        }
    }
}
