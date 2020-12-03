using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NaijaConnect.ViewModels
{
    public class SearchPageViewModel : ViewModelBase
    {
        public ObservableCollection<String> Topics { get => GetTopics(); }



        private DelegateCommand searrchCommand;

        public DelegateCommand NavigateToSearchCommand => searrchCommand ?? (searrchCommand = new DelegateCommand(ExecuteNavigateToSearchCommand));

        public SearchPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }

        private ObservableCollection<string> GetTopics()
        {
            var topics = new ObservableCollection<String>();
            topics.Add("Thermodynamics");
            topics.Add("Circuit theory");
            topics.Add("Control theory");
            topics.Add("Electromagnetics");
            topics.Add("Fluid Theory");
            topics.Add("Workshop");
            topics.Add("Thermodynamics");
            topics.Add("Thermodynamics");
            topics.Add("Circuit theory");
            topics.Add("Control theory");
            topics.Add("Electromagnetics");
            topics.Add("Fluid Theory");
            topics.Add("Workshop");
            //topics.Add("Thermodynamics");

            return topics;
        }

        private void ExecuteNavigateToSearchCommand()
        {
            NavigationService.NavigateAsync("MainSearchPage");
        }

    }
}
