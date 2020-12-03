using NaijaConnect.Models;
using NaijaConnect.Services;
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

namespace NaijaConnect.ViewModels
{
    public class CreateNewGroupPageViewModel : ViewModelBase
    {
        RemoteService remoteService;
        MediaFile mediaFile;
        string a;
        string groupPic;

        private DelegateCommand CreateGroupCommand;
        private DelegateCommand PickGroupAvatarCommand;
        public DelegateCommand NavigateCreateGroupCommand => CreateGroupCommand ?? (CreateGroupCommand = new DelegateCommand(ExecuteCreateGroupCommand));
        public DelegateCommand NavigatePickGroupAvatarCommand => PickGroupAvatarCommand ?? (PickGroupAvatarCommand = new DelegateCommand(ExecutePickGroupAvatarCommand));

        

        private Image avatar = new Image();

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

        private string groupName;

        public string GroupName
        {
            get
            {
                return groupName;
            }
            set
            {
                SetProperty(ref groupName, value);
            }
        }

        private string description;

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                SetProperty(ref description, value);
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




        public CreateNewGroupPageViewModel(INavigationService navigationService)
            :base(navigationService)
        {
            Title = "Create New Group";
            remoteService = new RemoteService();
            Avatar.Source = "default_profile_picture.png";
        }

        private async void ExecutePickGroupAvatarCommand()
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

            Avatar.Source = ImageSource.FromStream(() =>
            {
                var stream = mediaFile.GetStream();
                a = "1";
                return stream;
            });
        }

        private async void ExecuteCreateGroupCommand()
        {
            //checking for connectivity
            var network = Connectivity.NetworkAccess;
            if (network != NetworkAccess.Internet)
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Error: No internet access", actionButtonText: "OK", msDuration: 3000, configuration: App.snackBarConfiguration);
                return;
            }

            //checking for null
            if (string.IsNullOrEmpty(GroupName) || string.IsNullOrEmpty(Description))
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Error: Fill the required fields", actionButtonText: "OK", msDuration: 3000, configuration: App.snackBarConfiguration);
                return;
            }

            IsBusy = true;

            if (a == "1")
            {
                groupPic = await remoteService.UploadFile(mediaFile.GetStream(), Path.GetFileName(mediaFile.Path));
            }
            else
            {
                groupPic = "https://firebasestorage.googleapis.com/v0/b/naijaconnect-3b414.appspot.com/o/Naija%20Connect%20Images%2Fgroup_chat_picture.png?alt=media&token=547a5d56-3c2c-4811-849c-14a7f55132ed";
            }

            var group = new Group()
            {
                Name = GroupName.Trim(),
                Description = Description.Trim(),
                Avatar = groupPic
            };

            var createGroup = await remoteService.CreateGroup(group);

            if (!string.IsNullOrEmpty(createGroup))
            {
                var final = new Group()
                {
                    Name = GroupName.Trim(),
                    Description = Description.Trim(),
                    Avatar = groupPic,
                    Id = createGroup
                };


                var create = await remoteService.UpdateGroup(final);
                IsBusy = false;
                if (create)
                {
                    var parameters = new NavigationParameters();
                    parameters.Add("groupName", final.Name);
                    parameters.Add("groupId", final.Id);
                    parameters.Add("groupAvatar", final.Avatar);
                    await NavigationService.NavigateAsync("GroupChatPage", parameters);
                }
                else
                {
                    await MaterialDialog.Instance.SnackbarAsync(message: "Error occured", actionButtonText: "OK", msDuration: 3000, configuration: App.snackBarConfiguration);

                    return;
                }

            }
            else
            {
                IsBusy = false;
                await MaterialDialog.Instance.SnackbarAsync(message: "Error occured", actionButtonText: "OK", msDuration: 3000, configuration: App.snackBarConfiguration);
                
                return;
            }
        }
    }
}
