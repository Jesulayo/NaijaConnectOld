using NaijaConnect.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace NaijaConnect.ViewModels
{
    public class FriendProfilePageViewModel : ViewModelBase
    {
        RemoteService remoteService;
        public string email;
        public string username;
        public string phoneNumber;
        public string university;
        public string department;
        public bool isBusy;
        public Image avatar = new Image();

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                SetProperty(ref email, value);
            }
        }
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

        public string PhoneNumber
        {
            get
            {
                return phoneNumber;
            }
            set
            {
                SetProperty(ref phoneNumber, value);
            }
        }

        public string University
        {
            get
            {
                return university;
            }
            set
            {
                SetProperty(ref university, value);
            }
        }

        public string Department
        {
            get
            {
                return department;
            }
            set
            {
                SetProperty(ref department, value);
            }
        }

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                SetProperty(ref isBusy, value);
            }
        }

        public Image Avatar
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

        public FriendProfilePageViewModel(INavigationService navigationService)
            :base(navigationService)
        {
            remoteService = new RemoteService();
            
        }

        private async void GetUserProfile()
        {
            //checking for connectivity
            var network = Connectivity.NetworkAccess;
            if (network != NetworkAccess.Internet)
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Error: No internet access", actionButtonText: "OK", msDuration: 3000, configuration: App.snackBarConfiguration);
                return;
            }

            IsBusy = true;

            var userprofile = await remoteService.GetProfilePicture(Username);

            IsBusy = false;

            if(userprofile != null)
            {
                Email = userprofile.Email;
                PhoneNumber = userprofile.PhoneNumber;
                University = userprofile.University;
                Department = userprofile.Department;
                Avatar.Source = userprofile.Avatar;
            }

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Username = parameters.GetValue<string>("username");
            Avatar.Source = parameters.GetValue<string>("avatar");

            GetUserProfile();
        }

    }
}
