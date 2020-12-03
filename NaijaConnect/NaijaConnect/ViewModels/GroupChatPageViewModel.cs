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
    public class GroupChatPageViewModel : ViewModelBase
    {
        RemoteService remoteService;
        private DelegateCommand PreviousPageCommand;
        private DelegateCommand SendMessageCommand;

        public DelegateCommand NavigateToPreviousPageCommand => PreviousPageCommand ?? (PreviousPageCommand = new DelegateCommand(PreviousPage));
        public DelegateCommand NavigateToSendMessageCommand => SendMessageCommand ?? (SendMessageCommand = new DelegateCommand(SendMessage));

        private Group GroupDetails;
        public ObservableCollection<GroupChat> GroupChats { get; set; }
        public ObservableCollection<string> Users { get; set; }


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

        private string groupAvatar;
        public string GroupAvatar
        {
            get
            {
                return groupAvatar;
            }
            set
            {
                SetProperty(ref groupAvatar, value);
            }
        }

        private string groupId;
        public string GroupId
        {
            get
            {
                return groupId;
            }
            set
            {
                SetProperty(ref groupId, value);
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



        public GroupChatPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            remoteService = new RemoteService();
            GroupChats = new ObservableCollection<GroupChat>();
            Users = new ObservableCollection<string>();
            RetrieveMessage();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("nav"))
            {
                GroupDetails = parameters.GetValue<Group>("nav");
                GroupName = GroupDetails.Name;
                GroupAvatar = GroupDetails.Avatar;
                GroupId = GroupDetails.Id;

            }
            else
            {
                GroupName = parameters.GetValue<string>("groupName");
                GroupAvatar = parameters.GetValue<string>("groupAvatar");
                GroupId = parameters.GetValue<string>("groupId");
            }
        }

        private async void PreviousPage()
        {
            await NavigationService.GoBackAsync();
        }

        private async void SendMessage()
        {
            if (string.IsNullOrEmpty(Message) || string.IsNullOrWhiteSpace(Message))
            {
                Message = null;
                return;
            }
            var groupchat = new GroupChat()
            {
                SenderName = Settings.UsernameSettings,
                SenderId = Settings.KeySettings,
                GroupName = GroupName,
                GroupId = GroupId,
                GroupAvatar = GroupAvatar,
                Time = DateTime.Now,
                Message = Message.Trim(),
            };

            Message = null;
            GroupChats.Add(groupchat);
            var send = await remoteService.SendGroupMessage(groupchat);

        }

        private async void RetrieveMessage()
        {
            var groupChats = await remoteService.RetrieveGroupMessage();

            GroupChats.Clear();

            foreach (var item in groupChats)
            {
                if (item.GroupId == GroupId)
                {
                    GroupChats.Add(item);
                    if(Users.Count == 0)
                    {
                        Users.Add(item.SenderName);
                    }
                    else
                    {
                        if (!Users.Contains(item.SenderName))
                        {
                            Users.Add(item.SenderName);
                        }
                    }


                }
            }
        }


        //private ObservableCollection<Users> GetUsers()
        //{
        //    var userList = new ObservableCollection<Users>();
        //    userList.Add(new Users
        //    {
        //        Username = "Alakori"
        //    });
        //    userList.Add(new Users
        //    {
        //        Username = "Ade"
        //    });
        //    userList.Add(new Users
        //    {
        //        Username = "Peter"
        //    });
        //    userList.Add(new Users
        //    {
        //        Username = "Susan"
        //    });
        //    userList.Add(new Users
        //    {
        //        Username = "John"
        //    });
        //    userList.Add(new Users
        //    {
        //        Username = "Biggy"
        //    });

        //    return userList;

        //}

    }
}