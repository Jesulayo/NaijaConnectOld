using NaijaConnect.Models;
using NaijaConnect.Models.Model;
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
    public class ChatListPageViewModel : ViewModelBase
    {
        RemoteService remoteService;
        
        private DelegateCommand NewChatPageCommand;
        private DelegateCommand ChatSelectionCommand;
        private DelegateCommand _delegateRefreshCommand;
        private DelegateCommand ProfileCommand;
        

        public DelegateCommand NavigateNewChatPageCommand => NewChatPageCommand ?? (NewChatPageCommand = new DelegateCommand(ExecuteNavigateToNewChatPageCommand));
        public DelegateCommand NavigateChatSelectionCommand => ChatSelectionCommand ?? (ChatSelectionCommand = new DelegateCommand(ExecuteNavigateToChatSelectionCommand));
        public DelegateCommand NavigateRefreshCommand => _delegateRefreshCommand ?? (_delegateRefreshCommand = new DelegateCommand(RetrieveMessage));
        public DelegateCommand NavigateProfileCommand => ProfileCommand ?? (ProfileCommand = new DelegateCommand(ExecuteNavigateToProfileCommand));



        public ObservableCollection<DisplayChat> ChatList { get; set; }
        public ObservableCollection<string> ChatUsernameList { get; set; }

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

        private DisplayChat chatSelection;
        public DisplayChat ChatSelection
        {
            get
            {
                return chatSelection;
            }
            set
            {
                SetProperty(ref chatSelection, value);
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


        public ChatListPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Username = Settings.UsernameSettings;
            Avatar = Settings.AvatarSettings;
            remoteService = new RemoteService();
            ChatList = new ObservableCollection<DisplayChat>();
            ChatUsernameList = new ObservableCollection<string>();
            RetrieveMessage();
        }

        private async void RetrieveMessage()
        {
            IsBusy = true;
            var chats = await remoteService.RetrieveMessage();

            chats.Reverse();
            ChatUsernameList.Clear();
            ChatList.Clear();

            IsBusy = false;
            foreach (var item in chats)
            {
                if (item.SenderId == Settings.KeySettings)
                {
                    var chat = new DisplayChat()
                    {
                        Name = item.Receiver,
                        Id = item.ReceiverId,
                        //Email = item.ReceiverEmail,
                        Message = item.Message,
                        Avatar = item.ReceiverAvatar
                    };
                    if (ChatUsernameList.Count == 0 && ChatList.Count == 0)
                    {
                        ChatUsernameList.Add(item.Receiver);
                        ChatList.Add(chat);
                    }
                    else
                    {
                        if (!ChatUsernameList.Contains(item.Receiver))
                        {
                            ChatUsernameList.Add(item.Receiver);
                            ChatList.Add(chat);
                        }

                    }
                }
                else if (item.ReceiverId == Settings.KeySettings)
                {
                    var chat = new DisplayChat()
                    {
                        Name = item.Sender,
                        Id = item.SenderId,
                        //Email = item.SenderEmail,
                        Message = item.Message,
                        Avatar = item.SenderAvatar
                    };

                    if (ChatUsernameList.Count == 0 && ChatList.Count == 0)
                    {
                        ChatUsernameList.Add(item.Sender);
                        ChatList.Add(chat);
                    }
                    else
                    {
                        if (!ChatUsernameList.Contains(item.Sender))
                        {
                            ChatUsernameList.Add(item.Sender);
                            ChatList.Add(chat);
                        }
                    }
                }

                    
            }
        }
        private void ExecuteNavigateToNewChatPageCommand()
        {
            NavigationService.NavigateAsync("NewChatPage");
        }
        

        private async void ExecuteNavigateToChatSelectionCommand()
        {
            if (ChatSelection != null)
            {
                var parameters = new NavigationParameters();
                parameters.Add("navFromChatList", ChatSelection);
                await NavigationService.NavigateAsync("ChatPage", parameters);
                ChatSelection = null;
            }

        }

        private void ExecuteNavigateToProfileCommand()
        {
            NavigationService.NavigateAsync("ProfilePage");
        }

    }
}
