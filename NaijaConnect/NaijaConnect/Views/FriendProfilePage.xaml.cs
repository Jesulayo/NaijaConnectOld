using NaijaConnect.PopupPages;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace NaijaConnect.Views
{
    public partial class FriendProfilePage : ContentPage
    {
        public FriendProfilePage()
        {
            InitializeComponent();
        }

        private async void MyProfilePic_Tapped(object sender, System.EventArgs e)
        {
            var imageSender = (Image)sender;

            string name = username.Text;
            await Navigation.PushPopupAsync(new ChatProfileSelectionPopupView(imageSender.Source, name));
        }
    }
}
