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
using Xamarin.Essentials;

namespace NaijaConnect.ViewModels
{
    public class ChatPageViewModel : ViewModelBase
    {
        RemoteService remoteService;

        public ObservableCollection<Chat> Chats { get; set; }

        //public ObservableCollection<Messages> MyChats { get => GetMyChats(); }

        private DelegateCommand ProfileCommand;
        private DelegateCommand PreviousPageCommand;
        private DelegateCommand SendCommand;

        public DelegateCommand NavigateToProfileCommand => ProfileCommand ?? (ProfileCommand = new DelegateCommand(CheckProfile));
        public DelegateCommand NavigateToPreviousPageCommand => PreviousPageCommand ?? (PreviousPageCommand = new DelegateCommand(PreviousPage));
        public DelegateCommand NavigateToSendCommandCommand => SendCommand ?? (SendCommand = new DelegateCommand(SendMessage));

        User UserProfile;
        DisplayChat UserFromChatList;

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

        private string id;
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                SetProperty(ref id, value);
            }
        }

        private string message;
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                SetProperty(ref message, value);
            }
        }

        private string status;
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                SetProperty(ref status, value);
            }
        }

        public ChatPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            remoteService = new RemoteService();
            Chats = new ObservableCollection<Chat>();
            RetrieveMessage();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("nav"))
            {
                UserProfile = parameters.GetValue<User>("nav");
                Username = UserProfile.Username;
                Avatar = UserProfile.Avatar;
                Id = UserProfile.Id;
                Email = UserProfile.Email;
            }
            if (parameters.ContainsKey("navFromChatList"))
            {
                UserFromChatList = parameters.GetValue<DisplayChat>("navFromChatList");
                Username = UserFromChatList.Name;
                Email = UserFromChatList.Email;
                Avatar = UserFromChatList.Avatar;
                Id = UserFromChatList.Id;
                
            }

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var profilepic = await remoteService.GetProfilePicture(Username);
                if(profilepic != null)
                {
                   
                    Avatar = profilepic.Avatar;
                    Status = profilepic.Status;
                    
                }

            });


        }

        private async void PreviousPage()
        {
            await NavigationService.GoBackAsync();
        }
        private async void CheckProfile()
        {
            var parameters = new NavigationParameters();
            parameters.Add("username", Username);
            parameters.Add("id", Id);
            parameters.Add("avatar", Avatar);

            await NavigationService.NavigateAsync("FriendProfilePage", parameters); ;

        }
        private async void SendMessage()
        {
            if (string.IsNullOrEmpty(Message) || string.IsNullOrWhiteSpace(Message))
            {
                Message = null;
                return;
            }
            var chat = new Chat()
            {
                Sender = Settings.UsernameSettings,
                
                //SenderEmail = Settings.EmailSettings,
                SenderId = Settings.KeySettings,
                SenderAvatar = Settings.AvatarSettings,
                Message = Message.Trim(),
                Receiver = Username,
                ReceiverId = Id,
                //ReceiverEmail = Email,
                ReceiverAvatar = Avatar,
                Time = DateTime.Now
            };

            Message = null;
            Chats.Add(chat);
            var send = await remoteService.SendMessage(chat);
            
            
        }

        private async void RetrieveMessage()
        {
            var chats = await remoteService.RetrieveMessage();

            Chats.Clear();

            foreach (var item in chats)
            {
                if((item.SenderId == Settings.KeySettings && item.ReceiverId == Id) || (item.SenderId == Id && item.ReceiverId == Settings.KeySettings))
                {
                    Chats.Add(item);

                }
            }
        }


    }
}

