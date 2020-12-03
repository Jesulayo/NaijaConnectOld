using NaijaConnect.Services;
using NaijaConnect.Utils;
using NaijaConnect.ViewModels;
using NaijaConnect.Views;
using Prism;
using Prism.Ioc;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs.Configurations;

namespace NaijaConnect
{
    public partial class App
    {
        RemoteService remoteService;
        //Snack bar configuration
        public static MaterialSnackbarConfiguration snackBarConfiguration;

        public App() : this(null)
        {
            remoteService = new RemoteService();
        }
        //public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);
            StartUp();

            //await NavigationService.NavigateAsync("NavigationPage/CreateNewGroupPage");

            //Snack bar configuration
            snackBarConfiguration = new MaterialSnackbarConfiguration()
            {
                MessageTextColor = Color.FromHex("FFFFFF"),
                TintColor = Color.Red.MultiplyAlpha(0.8),
                CornerRadius = 5
            };
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<OnboardingPage, OnboardingPageViewModel>();
            containerRegistry.RegisterForNavigation<SignInPage, SignInPageViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpPageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<GroupChatListPage, GroupChatListPageViewModel>();
            containerRegistry.RegisterForNavigation<ChatListPage, ChatListPageViewModel>();
            containerRegistry.RegisterForNavigation<SearchPage, SearchPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<GroupChatPage, GroupChatPageViewModel>();
            containerRegistry.RegisterForNavigation<GroupChatPage, GroupChatPageViewModel>();
            containerRegistry.RegisterForNavigation<NewChatPage, NewChatPageViewModel>();
            containerRegistry.RegisterForNavigation<ChatPage, ChatPageViewModel>();
            containerRegistry.RegisterForNavigation<MainSearchPage, MainSearchPageViewModel>();
            containerRegistry.RegisterForNavigation<SearchUser, SearchUserViewModel>();
            containerRegistry.RegisterForNavigation<FriendProfilePage, FriendProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<NewGroupChatPage, NewGroupChatPageViewModel>();
            containerRegistry.RegisterForNavigation<CreateNewGroupPage, CreateNewGroupPageViewModel>();
        }

        public void StartUp()
        {
            if (string.IsNullOrEmpty(Settings.KeySettings))
            {
                NavigationService.NavigateAsync("OnboardingPage");
            }
            else
            {
                NavigationService.NavigateAsync("NavigationPage/HomePage");
            }
        }

        private async void status(string status)
        {
            await remoteService.UpdateStatus(status);

        }
        //public App()
        //{
        //    InitializeComponent();

        //    MainPage = new MainPage();
        //}

        protected override void OnStart()
        {
            base.OnResume();
            if (!string.IsNullOrEmpty(Settings.KeySettings))
            {
                status("online");

            }
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            if (!string.IsNullOrEmpty(Settings.KeySettings))
            {
                status("offline");

            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (!string.IsNullOrEmpty(Settings.KeySettings))
            {
                status("online");

            }


        }
    }
}
