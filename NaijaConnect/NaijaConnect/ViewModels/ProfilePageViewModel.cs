using NaijaConnect.Models;
using NaijaConnect.Services;
using NaijaConnect.Utils;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;

namespace NaijaConnect.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
    {
        RemoteService remoteService; 
        private DelegateCommand LogOutCommand;
        private DelegateCommand ChangeProfilePictureCommand;
        private DelegateCommand EditProfileCommand;
        private DelegateCommand UpdateUserCommand;

        public DelegateCommand NavigateToLogOutCommand => LogOutCommand ?? (LogOutCommand = new DelegateCommand(ExecuteLogOutCommand));
        public DelegateCommand NavigateToChangeProfilePictureCommand => ChangeProfilePictureCommand ?? (ChangeProfilePictureCommand = new DelegateCommand(ExecuteChangeProfilePictureCommand));
        public DelegateCommand NavigateEditProfileCommand => EditProfileCommand ?? (EditProfileCommand = new DelegateCommand(ExecuteEditProfileCommand));
        public DelegateCommand NavigateToUpdateUserCommand => UpdateUserCommand ?? (UpdateUserCommand = new DelegateCommand(ExecuteUpdateUserCommand));

        

        MediaFile mediaFile;
        public string email;
        public string username;
        public string phoneNumber;
        public string university;
        public string department;
        public bool isBusy;
        public bool isNotBusy;
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



        public ProfilePageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Profile";
            remoteService = new RemoteService();
            Email = Settings.EmailSettings;
            Username = Settings.UsernameSettings;
            PhoneNumber = Settings.PhoneNumberSettings;
            University = Settings.UniversitySettings;
            Department = Settings.DepartmentSettings;
            Avatar.Source = Settings.AvatarSettings;
        }

        private async void ExecuteLogOutCommand()
        {
            await remoteService.UpdateStatus("offline");

            Settings.ClearAllData();
            
            await NavigationService.NavigateAsync("/SignInPage");
        }

        private async void ExecuteChangeProfilePictureCommand()
        {
            await CrossMedia.Current.Initialize();

            //choosse - from gallery or camera
            bool choice = await App.Current.MainPage.DisplayAlert("Confirm", "Where do you want to select your image from", "Camera", "Gallery");
            
            //camera
            if (choice)
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Camera not supported", "OK");
                    return;
                }

                mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Large
                });

                if (mediaFile == null)
                {
                    //await App.Current.MainPage.DisplayAlert("Error", "Camera not supported", "OK");
                    return;
                }

            }
            //gallery
            else
            {

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Pick photo not supported", "OK");
                    return;
                }

                var mediaOptions = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Large
                };


                mediaFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

                if (mediaFile == null)
                {
                    //await App.Current.MainPage.DisplayAlert("Error", "Pick photo not supported", "OK");
                    return;
                }
            }

            //checking for connectivity
            var network = Connectivity.NetworkAccess;
            if (network != NetworkAccess.Internet)
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Error: No internet access", actionButtonText: "OK", msDuration: 3000, configuration: App.snackBarConfiguration);
                return;
            }

            IsBusy = true;
            //save image to online storage
            var profilePicUrl = await remoteService.UploadFile(mediaFile.GetStream(), Path.GetFileName(mediaFile.Path));
            
            //if storing image fail
            if(profilePicUrl == null)
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Error: Unable to change profile picture", actionButtonText: "OK", msDuration: 3000, configuration: App.snackBarConfiguration);
                return;

            }

            //update profile picture in database
            var updateProfilePic = await remoteService.UpdateProfilePicture(profilePicUrl);
            IsBusy = false;
            if (updateProfilePic)
            {
                Avatar.Source = ImageSource.FromStream(() =>
                {
                    var stream = mediaFile.GetStream();
                    return stream;
                });

                Settings.AvatarSettings = profilePicUrl;
            }
            else
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Error occured", actionButtonText: "OK", msDuration: 3000, configuration: App.snackBarConfiguration);
                return;
            }

        }

        private void ExecuteEditProfileCommand()
        {
            IsNotBusy = true;
        }

        private async void ExecuteUpdateUserCommand()
        {
            var network = Connectivity.NetworkAccess;
            if (network != NetworkAccess.Internet)
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Error: No internet access", actionButtonText: "OK", msDuration: 3000, configuration: App.snackBarConfiguration);
                return;
            }


            if (Username == null ||  PhoneNumber == null || University == null || Department == null)
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Error: Fill the required fields", actionButtonText: "OK", msDuration: 3000, configuration: App.snackBarConfiguration);
                return;
            }

            var loadingPageConfiguration = new MaterialLoadingDialogConfiguration()
            {
                BackgroundColor = Color.FromHex("048B28"),
                MessageTextColor = Color.FromHex("FFFFFF"),
                CornerRadius = 10,
                TintColor = Color.FromHex("FFFFFF"),
                ScrimColor = Color.FromHex("1DA1F2").MultiplyAlpha(0.05)
            };

            var loadingDialog = await MaterialDialog.Instance.LoadingDialogAsync(message: "Processing", configuration: loadingPageConfiguration);

            var updateUser = new User
            {
                Email = Settings.EmailSettings,
                Username = this.Username.Trim(),
                PhoneNumber = this.PhoneNumber.Trim(),
                University = this.University.Trim(),
                Department = this.Department.Trim(),
                Id = Settings.KeySettings,
                Avatar = Settings.AvatarSettings
            };

            var user = await remoteService.UpdateUser(updateUser);
            await loadingDialog.DismissAsync();
            if (user)
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Profile Updated successfully", actionButtonText: "OK", msDuration: 2000, configuration: App.snackBarConfiguration);
                IsNotBusy = false;
                Settings.UsernameSettings = Username.Trim();
                Settings.PhoneNumberSettings = PhoneNumber.Trim();
                Settings.UniversitySettings = University.Trim();
                Settings.DepartmentSettings = Department.Trim();
            }
            else
                await MaterialDialog.Instance.SnackbarAsync(message: "Update failed", actionButtonText: "OK", msDuration: 2000, configuration: App.snackBarConfiguration);


        }

    }
}
