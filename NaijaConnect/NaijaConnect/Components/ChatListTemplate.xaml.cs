using NaijaConnect.PopupPages;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NaijaConnect.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatListTemplate : ContentView
    {
        public ChatListTemplate()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var imageSender = (Image)sender;

            string name = username.Text;
            await Navigation.PushPopupAsync(new ChatProfileSelectionPopupView(imageSender.Source, name));


            //await Navigation.PushAsync(new NewChatPage());
        }
    }
}