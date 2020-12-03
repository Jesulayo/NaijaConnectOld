using System;
using System.Collections.Generic;
using System.Text;

namespace NaijaConnect.Models
{
    public class GroupChat
    {
        public string SenderName { get; set; }
        public string SenderId { get; set; }
        public string GroupName { get; set; }
        public string GroupId { get; set; }
        public string GroupAvatar { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
    }
}
