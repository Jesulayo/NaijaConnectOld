using System;
using System.Collections.Generic;
using System.Text;

namespace NaijaConnect.Models
{
    public class Chat
    {
        public string Sender { get; set; }
        public string SenderId { get; set; }
        //public string SenderEmail { get; set; }
        public string SenderAvatar { get; set; }
        public string Message { get; set; }
        public string Receiver { get; set; }
        public string ReceiverId { get; set; }
        //public string ReceiverEmail { get; set; }
        public string ReceiverAvatar { get; set; }
        public DateTime Time { get; set; }
    }
}
