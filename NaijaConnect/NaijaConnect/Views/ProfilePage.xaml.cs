using NaijaConnect.PopupPages;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace NaijaConnect.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            save.Opacity = 0;
        }

        private async void MyProfilePic_Tapped(object sender, System.EventArgs e)
        {
            var imageSender = (Image)sender;

            string name = username.Text;
            await Navigation.PushPopupAsync(new ChatProfileSelectionPopupView(imageSender.Source, name));
        }

        private void Edit_Tapped(object sender, System.EventArgs e)
        {
            logout.Opacity = 0;
            save.Opacity = 1;
        }

        private void Save_Tapped(object sender, System.EventArgs e)
        {
            logout.Opacity = 1;
            save.Opacity = 0;
        }
    }
}
