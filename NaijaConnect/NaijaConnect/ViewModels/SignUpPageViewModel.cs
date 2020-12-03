using Firebase.Auth;
using NaijaConnect.Models;
using NaijaConnect.Services;
using NaijaConnect.Styles;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;

namespace NaijaConnect.ViewModels
{
    public class SignUpPageViewModel : ViewModelBase
    {
        RemoteService remoteService;
        private DelegateCommand signInPageCommand;
        private DelegateCommand signUpCommand;

        public DelegateCommand SignInPageCommand => signInPageCommand ?? (signInPageCommand = new DelegateCommand(ExecuteSignInPageCommand));
        public DelegateCommand SignUpCommand => signUpCommand ?? (signUpCommand = new DelegateCommand(ExecuteSignUpCommand));

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

        private string phoneNumber;
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

        private string university;
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

        private string department;
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

        private string confirmPassword;
        public string ConfirmPassword
        {
            get
            {
                return confirmPassword;
            }
            set
            {
                SetProperty(ref confirmPassword, value);
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
        private double opacity;
        public double Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                SetProperty(ref opacity, value);
            }
        }


        public SignUpPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            IsNotBusy = true;
            Opacity = 1;
            remoteService = new RemoteService();
        }

        private void ExecuteSignInPageCommand()
        {
            NavigationService.NavigateAsync("SignInPage");
        }

        private async void ExecuteSignUpCommand()
        {

            //checking for connectivity
            var network = Connectivity.NetworkAccess;
            if (network != NetworkAccess.Internet)
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Error: No internet access", actionButtonText: "OK", msDuration: 3000, configuration: App.snackBarConfiguration);
                return;
            }

            //checking for null
            if (Email == null || Username == null || PhoneNumber == null || University == null || Department == null || Password == null || ConfirmPassword == null)
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Error: Fill the required fields", actionButtonText: "OK", msDuration: 3000, configuration: App.snackBarConfiguration);
                return;
            }

            //password length
            if (Password.Length < 6)
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Error: Password length must be at least 6 characters", actionButtonText: "OK", msDuration: 3000, configuration: App.snackBarConfiguration);
                return;
            }

            if (Password.ToLower().Trim() != ConfirmPassword.ToLower().Trim())
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Error: Passwords do not match", actionButtonText: "OK", msDuration: 3000, configuration: App.snackBarConfiguration);

                return;
            }

            //loading dialog configuration
            var loadingPageConfiguration = new MaterialLoadingDialogConfiguration()
            {
                BackgroundColor = Color.FromHex("048B28"),
                MessageTextColor = Color.FromHex("FFFFFF"),
                CornerRadius = 10,
                TintColor = Color.FromHex("FFFFFF"),
                ScrimColor = Color.FromHex("1DA1F2").MultiplyAlpha(0.05)
            };

            //opacity of the page
            Opacity = 0.8;

            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Processing", configuration: loadingPageConfiguration);

            IsNotBusy = false;

            //remote service -  create  user
            var user = await remoteService.SignUp(Email.ToLower().Trim(), Password.ToLower().Trim());


            if (user.Id != null)
                {
                var signUp = new SignUp()
                {
                    Email = this.Email.ToLower().Trim(),
                    Username = this.Username.Trim(),
                    PhoneNumber = this.PhoneNumber.Trim(),
                    University = this.University.Trim(),
                    Department = this.Department.Trim(),
                    Id = user.Id,
                    Avatar = "https://firebasestorage.googleapis.com/v0/b/naijaconnect-3b414.appspot.com/o/default%20profile%20picture.png?alt=media&token=092092f9-886d-4a3c-8271-bbbcecc5678d",
                    Status = "offline"
                };

                //save user to database
                var completeSignUp = await remoteService.SaveUser(signUp);

                await loadingDialog.DismissAsync();

                 if (completeSignUp)
                 {
                    Email = null;
                    Username = null;
                    PhoneNumber = null;
                    University = null;
                    Department = null;
                    Password = null;
                    ConfirmPassword = null;

                    IsNotBusy = true;
                    Opacity = 1;
                    await MaterialDialog.Instance.SnackbarAsync(message: "Account created successfully", actionButtonText: "OK", msDuration: 2000, configuration: App.snackBarConfiguration);
                    
                    await NavigationService.NavigateAsync("SignInPage");
                 }
                else
                {
                    await MaterialDialog.Instance.SnackbarAsync(message: "Error occured", actionButtonText: "OK", msDuration: 2000, configuration: App.snackBarConfiguration);

                }
            }

            else
            {
                await loadingDialog.DismissAsync();

                await MaterialDialog.Instance.SnackbarAsync(message: "Error: " + user.ErrorMessage, actionButtonText: "OK", msDuration: 2000, configuration: App.snackBarConfiguration);
            }

            
        }
    }
}
