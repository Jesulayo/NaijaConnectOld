using NaijaConnect.Models;
using NaijaConnect.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NaijaConnect.ViewModels
{
    public class NewGroupChatPageViewModel : ViewModelBase
    {
        RemoteService remoteService;

        private DelegateCommand DelegateSelectionCommand;
        private DelegateCommand CreateNewGroupCommand;

        public DelegateCommand NavigateCreateNewGroupCommand => CreateNewGroupCommand ?? (CreateNewGroupCommand = new DelegateCommand(ExecuteCreateNewGroupCommand));
        public DelegateCommand NavigateSelectionCommand => DelegateSelectionCommand ?? (DelegateSelectionCommand = new DelegateCommand(ExecuteSelectionCommand));


        public ObservableCollection<Group> AllGroups { get; set; }

        public Group groupSelection;

        public Group GroupSelection
        {
            get
            {
                return groupSelection;
            }
            set
            {
                SetProperty(ref groupSelection, value);
            }
        }

        public NewGroupChatPageViewModel(INavigationService navigationService)
            :base(navigationService)
        {
            remoteService = new RemoteService();
            AllGroups = new ObservableCollection<Group>();
            GetAllGroups();
        }

        private async void GetAllGroups()
        {
            try
            {
                var groups = await remoteService.GetAllGroup();
                AllGroups.Clear();
                foreach (var item in groups)
                {
                    AllGroups.Add(item);
                }
            }
            catch
            {

            }
        }

        private void ExecuteCreateNewGroupCommand()
        {
            NavigationService.NavigateAsync("CreateNewGroupPage");
        }

        private async void ExecuteSelectionCommand()
        {
            if (GroupSelection != null)
            {
                var parameters = new NavigationParameters();
                parameters.Add("nav", GroupSelection);
                await NavigationService.NavigateAsync("GroupChatPage", parameters);
                GroupSelection = null;
            }
        }
    }
}
