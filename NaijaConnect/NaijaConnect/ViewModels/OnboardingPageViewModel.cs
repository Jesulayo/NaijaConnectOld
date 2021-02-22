using NaijaConnect.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NaijaConnect.ViewModels
{
    public class OnboardingPageViewModel : ViewModelBase
    {
        private DelegateCommand signInCommand;
        private DelegateCommand signUpCommand;

        public DelegateCommand NavigateToSignInCommand => signInCommand ?? (signInCommand = new DelegateCommand(ExecuteNavigateToSignInCommand));
        public DelegateCommand NavigateToSignUpCommand => signUpCommand ?? (signUpCommand = new DelegateCommand(ExecuteNavigateToSignUpCommand));

        public ObservableCollection<Welcome> Welcomes { get => GetWelcomes(); }
        public OnboardingPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }

        private void ExecuteNavigateToSignInCommand()
        {
            NavigationService.NavigateAsync("SignInPage");
        }

        private void ExecuteNavigateToSignUpCommand()
        {
            NavigationService.NavigateAsync("SignUpPage");

        }

        private ObservableCollection<Welcome> GetWelcomes()
        {
            var welcomeList = new ObservableCollection<Welcome>();

            welcomeList.Add(new Welcome
            {
                Note = "Connect with other students",
                Picture = "OnboardingD.jpg"
            });
            welcomeList.Add(new Welcome
            {
                Note = "Chat with other students",
                Picture = "OnboardingB.jpg"
            });
            welcomeList.Add(new Welcome
            {
                Note = "Meet other students",
                Picture = "OnboardingC.jpg"
            });

            return welcomeList;
        }
    }
}
