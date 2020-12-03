using NaijaConnect.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaijaConnect.Services
{
    public class DataService
    {
        public ObservableCollection<NewChats> NewUsers { get; set; }

        public DataService()
        {
            NewUsers = new ObservableCollection<NewChats>();
            InitializeNewUsers();
        }
        public void InitializeUser()
        {
            NewUsers.Add(new NewChats("Alakori", "profilepic.jpeg", "Geophysics"));
            NewUsers.Add(new NewChats("Tundex", "profilepic.jpeg", "Biology"));
            NewUsers.Add(new NewChats("Biggy", "profilepic.jpeg", "Architecture"));

        }

        public ObservableCollection<NewChats> InitializeNewUsers()
        {
            var newUsers = new ObservableCollection<NewChats>();
            newUsers.Add(new NewChats
            {
                Username = "Alakori",
                ProfilePicture = "profilepic.jpeg",
                Department = "Geophysics"
            });
            newUsers.Add(new NewChats
            {
                Username = "Tundex",
                ProfilePicture = "profilepic.jpeg",
                Department = "Biology"
            });
            newUsers.Add(new NewChats
            {
                Username = "Biggy",
                ProfilePicture = "profilepic.jpeg",
                Department = "Architecture"
            });

            return newUsers;
        }

        public async Task<ObservableCollection<NewChats>> GetUsersAsync(string query)
        {
            //Thread.Sleep(2000);

            if (query != string.Empty)
            {
                NewUsers.Clear();
                InitializeUser();
                List<NewChats> users = NewUsers.Where(t => (t.Username.ToLower().Contains(query.ToLower())
                                                                    || t.Department.ToLower().Contains(query.ToLower()))).ToList();

                NewUsers.Clear();
                foreach (NewChats user in users)
                {
                    NewUsers.Add(user);
                }

            }
            else
            {
                NewUsers.Clear();
            }
            return await Task.FromResult(NewUsers);
        }

        
    }
}
