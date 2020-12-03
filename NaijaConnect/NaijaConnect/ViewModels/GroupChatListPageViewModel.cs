using NaijaConnect.Models;
using NaijaConnect.Models.Model;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NaijaConnect.ViewModels
{
    public class GroupChatListPageViewModel : ViewModelBase
    {
        private DelegateCommand GroupChatSelectionCommand;
        private DelegateCommand NewGroupChatPageCommand;
        public DelegateCommand NavigateGroupChatSelectionCommand => GroupChatSelectionCommand ?? (GroupChatSelectionCommand = new DelegateCommand(ExecuteNavigateToGroupChatSelectionCommand));
        public DelegateCommand NavigateNewGroupChatPageCommand => NewGroupChatPageCommand ?? (NewGroupChatPageCommand = new DelegateCommand(ExecuteNavigateToNewGroupChatPageCommand));

        

        public ObservableCollection<GroupModelss> MyGroupChat { get => GetGroupChats(); }

        private GroupModelss groupChatSelection;

        public GroupModelss GroupChatSelection
        {
            get
            {
                return groupChatSelection;
            }
            set
            {
                SetProperty(ref groupChatSelection, value);
            }
        }
        public GroupChatListPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Groups";
        }

        private ObservableCollection<GroupModelss> GetGroupChats()
        {
            var chatList = new ObservableCollection<GroupModelss>();
            chatList.Add(new GroupModelss
            {
                Groupname = "Tunde",
                ProfilePicture = "profilepic.jpeg",
                LastMessage = "That question hard die"
            });
            chatList.Add(new GroupModelss
            {
                Groupname = "Tunde",
                ProfilePicture = "profilepic.jpeg",
                LastMessage = "That question hard die"
            });
            chatList.Add(new GroupModelss
            {
                Groupname = "Tunde",
                ProfilePicture = "profilepic.jpeg",
                LastMessage = "That question hard die"
            });
            chatList.Add(new GroupModelss
            {
                Groupname = "Tunde",
                ProfilePicture = "profilepic.jpeg",
                LastMessage = "That question hard die"
            });
            
            return chatList;

        }

        private async void ExecuteNavigateToGroupChatSelectionCommand()
        {
            if (GroupChatSelection != null)
            {


                await NavigationService.NavigateAsync("GroupChatPage");


                GroupChatSelection = null;
            }
        }

        private void ExecuteNavigateToNewGroupChatPageCommand()
        {
            NavigationService.NavigateAsync("NewGroupChatPage");

        }
    }
}
