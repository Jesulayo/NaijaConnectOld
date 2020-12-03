using NaijaConnect.PopupPages;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace NaijaConnect.Views
{
    public partial class ChatPage : ContentPage
    {
        public ChatPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                this.SetBinding(HeightRequestProperty, new Binding("Height", BindingMode.OneWay, null, null, null, chatTextInput));
            }
        }

        //public void Handle_Completed(object sender, EventArgs e)
        //{
        //    (this.Parent.Parent.BindingContext as ChatPageViewModel).OnSendCommand.Execute(null);
        //    chatTextInput.Focus();
        //}

        //public void UnFocusEntry()
        //{
        //    chatTextInput?.Unfocus();
        //}

        private async void ProfilePic_Tapped(object sender, System.EventArgs e)
        {
            var imageSender = (Image)sender;

            string name = username.Text;
            await Navigation.PushPopupAsync(new ChatProfileSelectionPopupView(imageSender.Source, name));

        }
    }
}
