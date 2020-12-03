using System;
using System.Collections.Generic;
using System.Text;

namespace NaijaConnect.Models
{
    public class NewChats
    {
        public string Username { get; set; }
        public string ProfilePicture { get; set; }
        public string Department { get; set; }

        public NewChats(string username, string profilepicture, string department)
        {
            Username = username;
            ProfilePicture = profilepicture;
            Department = department;
        }

        public NewChats()
        {

        }
    }
}
