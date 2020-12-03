using NaijaConnect.Models;
using NaijaConnect.Models.Model;
using NaijaConnect.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NaijaConnect.CustomCell
{
    public class GroupMessageDataTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate incomingGroupMessageDataTemplate;
        private readonly DataTemplate outgoingGroupMessageDataTemplate;
        public GroupMessageDataTemplateSelector()
        {
            incomingGroupMessageDataTemplate = new DataTemplate(typeof(GroupIncomingMessage));
            outgoingGroupMessageDataTemplate = new DataTemplate(typeof(GroupOutgoingMessage));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messages = item as GroupChat;
            if (messages == null)
                return null;

            return messages.SenderId == Settings.KeySettings ? outgoingGroupMessageDataTemplate : incomingGroupMessageDataTemplate;
        }
    }
}
