using NaijaConnect.Models;
using NaijaConnect.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NaijaConnect.CustomCell
{
    public class ChatTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate incomingChatDataTemplate;
        private readonly DataTemplate outgoingChatDataTemplate;
        public ChatTemplateSelector()
        {
            // Retain instances!
            this.incomingChatDataTemplate = new DataTemplate(typeof(IncomingChat));
            this.outgoingChatDataTemplate = new DataTemplate(typeof(OutgoingChat));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as Chat;
            if (messageVm == null)
                return null;
            return messageVm.SenderId == Settings.KeySettings ? this.outgoingChatDataTemplate : this.incomingChatDataTemplate;
        }

        
    }
}

