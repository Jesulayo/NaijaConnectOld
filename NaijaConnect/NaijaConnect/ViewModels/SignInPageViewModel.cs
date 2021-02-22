using NaijaConnect.Services;
using NaijaConnect.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using XF.Material.Forms.UI.Dialogs;

namespace NaijaConnect.ViewModels
{
    public class SignInPageViewModel : ViewModelBase
    {
        RemoteService remoteService;
        private DelegateCommand SignInCommand;
        private DelegateCommand SignUpPageCommand;

        public DelegateCommand NavigateSignInCommand => SignInCommand ?? (SignInCommand = new DelegateCommand(ExecuteNavigateToSignInCommand));
        public DelegateCommand NavigateToSignUpPageCommand => SignUpPageCommand ?? (SignUpPageCommand = new DelegateCommand(ExecuteSignUpPageCommand));

        private string email;
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

        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                SetProperty(ref password, value);
            }
        }

        private bool isBusy;
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

        private bool isNotBusy;
        public bool IsNotBusy
        {
            get
            {
                return isNotBusy;
            }
            set
            {
                SetProperty(ref isNotBusy, value);
            }
        }


        public SignInPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            IsNotBusy = true;
            remoteService = new RemoteService();
        }

        private async void ExecuteNavigateToSignInCommand()
        {
            //checking for connectivity
            var network = Connectivity.NetworkAccess;
            if (network != NetworkAccess.Internet)
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Error: No internet access", actionButtonText: "OK", msDuration: 3000, configuration: App.snackBarConfiguration);
                return;
            }

            //checking for null
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Error: Fill the required fields", actionButtonText: "OK", msDuration: 3000, configuration: App.snackBarConfiguration);
                return;
            }

            IsNotBusy = false;
            IsBusy = true;
            //remote service - sign in
            var login = await remoteService.SignIn(Email.ToLower().Trim(), Password.ToLower().Trim());
            if (login.Id != null)
            {
                var user = await remoteService.LoginAsync(Email.ToLower().Trim(), login.Id);
                
                IsNotBusy = true;
                IsBusy = false;

                if(user != null)
                {
                    Settings.EmailSettings = user.Email;
                    Settings.UsernameSettings = user.Username;
                    Settings.PhoneNumberSettings = user.PhoneNumber;
                    Settings.UniversitySettings = user.University;
                    Settings.DepartmentSettings = user.Department;
                    Settings.AvatarSettings = user.Avatar;
                    Settings.KeySettings = user.Id;
                        

                    Email = null;
                    Password = null;
                    await NavigationService.NavigateAsync("/NavigationPage/ChatListPage");

                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await remoteService.UpdateStatus("online");

                    });

                }
                else
                    await MaterialDialog.Instance.SnackbarAsync(message: "Error: Error Occurred. Try again", actionButtonText: "OK", msDuration: 3000, configuration: App.snackBarConfiguration);

            }
            else
            {
                IsNotBusy = true;
                IsBusy = false;
                await MaterialDialog.Instance.SnackbarAsync(message: "Error: " + login.ErrorMessage, actionButtonText: "OK", msDuration: 2000, configuration: App.snackBarConfiguration);
            }

        }

        private void ExecuteSignUpPageCommand()
        {
            NavigationService.NavigateAsync("SignUpPage");
        }
    }
}
