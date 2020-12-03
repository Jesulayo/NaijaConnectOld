using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using NaijaConnect.Models;
using NaijaConnect.Models.Responses;
using NaijaConnect.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaijaConnect.Services
{
    public class RemoteService
    {
        const string CONFIG_KEY = "AIzaSyBoV_p7gd4gqofKlb0wbjykc33eK6HJmkg";
        FirebaseAuthProvider firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(CONFIG_KEY));

        FirebaseClient firebaseClient = new FirebaseClient("https://naijaconnect-3b414.firebaseio.com/");

        FirebaseStorage firebaseStorage = new FirebaseStorage("naijaconnect-3b414.appspot.com");



        public async Task<string> UploadFile(Stream fileStream, string fileName)
        {
            var imageUrl = await firebaseStorage
                .Child("Naija Connect Images")
                .Child(fileName)
                .PutAsync(fileStream);
           
         return imageUrl;
        }

        public async Task<Response> SignUp(string email, string password)
        {
            var response = new Response();
            try
            {

                var user = await firebaseAuthProvider.CreateUserWithEmailAndPasswordAsync(email, password);

                if (user.User.LocalId != null)
                {
                    Firebase.Auth.User x = user.User;
                    //string y = x.LocalId;
                    response.Id = user.User.LocalId;
                    return response;
                }
                else
                {
                    return null;
                }

            }
            //catch exception if account is not created
            catch (FirebaseAuthException e)
            {

                switch (e.Reason.ToString())
                {
                    case "EmailExists":
                        response.ErrorMessage = "Email already exists";
                        break;
                    case "InvalidEmailAddress":
                        response.ErrorMessage = "Invalid Email";
                        break;
                    default:
                        response.ErrorMessage = "Error Occured";
                        break;

                }
                return response;

            }

        }

        public async Task<bool> SaveUser(SignUp signUp)
        {
            try
            {
                await firebaseClient.Child("User").Child(signUp.Id).PutAsync(signUp);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<Response> SignIn(string email, string password)
        {
            var response = new Response();
            try
            {
                var login = await firebaseAuthProvider.SignInWithEmailAndPasswordAsync(email, password);
                if (login.User.LocalId != null)
                {
                    response.Id = login.User.LocalId;
                    Settings.TokenSettings = login.FirebaseToken;
                    return response;
                }
                else
                    return null;

            }
            catch (FirebaseAuthException e)
            {
                //var a = e.Reason.ToString();
                switch (e.Reason.ToString())
                {
                    case "UnknownEmailAddress":
                        response.ErrorMessage = "Account does not exist";
                        break;
                    case "WrongPassword":
                        response.ErrorMessage = "Wrong Password";
                        break;
                    case "UserDisabled":
                        response.ErrorMessage = "User Disabled";
                        break;
                    default:
                        response.ErrorMessage = "Error Occured";
                        break;
                }
                return response;
            }
        }

        public async Task<List<Models.User>> GetAllUser()
        {
           //var d = firebaseAuthProvider.GetUserAsync(Settings.TokenSettings);
            try
            {
                var userlist = (await firebaseClient
                .Child("User")
                .OnceAsync<Models.User>()).Select(item =>
                new Models.User
                {
                    Email = item.Object.Email,
                    Username = item.Object.Username,
                    PhoneNumber = item.Object.PhoneNumber,
                    University = item.Object.University,
                    Department = item.Object.Department,
                    Id = item.Object.Id,
                    Avatar = item.Object.Avatar,
                    Status = item.Object.Status
                }).ToList();
                
                return userlist;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        public async Task<Models.User> LoginAsync(string email, string id)
        {
            try
            {
                var user = await GetAllUser();
                await firebaseClient
                .Child("User")
                .OnceAsync<Models.User>();
                return user.Where(a => a.Email == email && a.Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        public async Task<Models.User> GetProfilePicture(string username)
        {
            try
            {
                var user = await GetAllUser();
                await firebaseClient
                .Child("User")
                .OnceAsync<Models.User>();
                return user.Where(a => a.Username == username).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }



        public async Task<bool> SendMessage(Chat chat)
        {
            try
            {
                var user = await firebaseClient
                    .Child("Chats")
                    .PostAsync(new Chat()
                    {
                        Sender = chat.Sender,
                        //SenderEmail = chat.SenderEmail,
                        SenderId = chat.SenderId,
                        SenderAvatar = chat.SenderAvatar,
                        Message = chat.Message,
                        Receiver = chat.Receiver,
                        //ReceiverEmail = chat.ReceiverEmail,
                        ReceiverId = chat.ReceiverId,
                        ReceiverAvatar = chat.ReceiverAvatar,
                        Time = chat.Time
                    });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Chat>> RetrieveMessage()
        {
            try
            {
                var messages = (await firebaseClient
                    .Child("Chats")
                    .OnceAsync<Chat>()).Select(item =>
                    new Chat
                    {
                        Sender = item.Object.Sender,
                        //SenderEmail = item.Object.SenderEmail,
                        SenderId = item.Object.SenderId,
                        SenderAvatar = item.Object.SenderAvatar,
                        Message = item.Object.Message,
                        Receiver = item.Object.Receiver,
                        //ReceiverEmail = item.Object.ReceiverEmail,
                        ReceiverId = item.Object.ReceiverId,
                        ReceiverAvatar = item.Object.ReceiverAvatar,
                        Time = item.Object.Time
                    }).ToList();
                return messages;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateProfilePicture(string imageUrl)
        {
            try
            {
                await firebaseClient
                    .Child("User")
                    .Child(Settings.KeySettings)
                    .PutAsync(new Models.User()
                    {
                        Email = Settings.EmailSettings,
                        Username = Settings.UsernameSettings,
                        PhoneNumber = Settings.PhoneNumberSettings,
                        University = Settings.UniversitySettings,
                        Department = Settings.DepartmentSettings,
                        Avatar = imageUrl,
                        Id = Settings.KeySettings,
                        Status = "online"
                    });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateUser(Models.User user)
        {
            try
            {

                await firebaseClient
                  .Child("User")
                  .Child(Settings.KeySettings)
                  .PutAsync(new Models.User()
                  {
                      Email = user.Email,
                      Username = user.Username,
                      PhoneNumber = user.PhoneNumber,
                      University = user.University,
                      Department = user.Department,
                      Avatar = user.Avatar,
                      Id = user.Id,
                      Status = "online"
                  });
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }

        }


        public async Task UpdateStatus(string status)
        {
            try
            {

                await firebaseClient
                  .Child("User")
                  .Child(Settings.KeySettings)
                  .PutAsync(new Models.User()
                  {
                      Email = Settings.EmailSettings,
                      Username = Settings.UsernameSettings,
                      PhoneNumber = Settings.PhoneNumberSettings,
                      University = Settings.UniversitySettings,
                      Department = Settings.DepartmentSettings,
                      Avatar = Settings.AvatarSettings,
                      Id = Settings.KeySettings,
                      Status = status
                  });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }


        public async Task<List<Group>> GetAllGroup()
        {
            try
            {
                var grouplist = (await firebaseClient
                .Child("Groups")
                .OnceAsync<Group>()).Select(item =>
                new Group
                {
                    Name = item.Object.Name,
                    Description = item.Object.Description,
                    Avatar = item.Object.Avatar,
                    Id = item.Key
                }).ToList();
                return grouplist;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        public async Task<string> CreateGroup(Group group)
        {
            try
            {
                var x = await firebaseClient
                .Child("Groups")
                .PostAsync(new Group()
                {
                    Name = group.Name,
                    Description = group.Description,
                    Avatar = group.Avatar
                    
                });
                return x.Key;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        public async Task<bool> UpdateGroup(Group group)
        {
            try
            {
                await firebaseClient
                  .Child("Groups")
                  .Child(group.Id)
                  .PutAsync(new Group()
                  {
                      Name = group.Name,
                      Description = group.Description,
                      Avatar = group.Avatar,
                      Id = group.Id
                  });
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }

        }

        public async Task<bool> SendGroupMessage(GroupChat groupChat)
        {
            try
            {
                await firebaseClient
                    .Child("GroupChats")
                    .PostAsync(new GroupChat()
                    {
                        SenderName = groupChat.SenderName,
                        SenderId = groupChat.SenderId,
                        GroupName = groupChat.GroupName,
                        GroupId = groupChat.GroupId,
                        GroupAvatar = groupChat.GroupAvatar,
                        Time = groupChat.Time,
                        Message = groupChat.Message
                    });
                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<List<GroupChat>> RetrieveGroupMessage()
        {
            try
            {
                var messages = (await firebaseClient
                    .Child("GroupChats")
                    .OnceAsync<GroupChat>()).Select(item =>
                    new GroupChat
                    {
                        SenderName = item.Object.SenderName,
                        SenderId = item.Object.SenderId,
                        GroupName = item.Object.GroupName,
                        GroupId = item.Object.GroupId,
                        GroupAvatar = item.Object.GroupAvatar,
                        Time = item.Object.Time,
                        Message = item.Object.Message,
                    }).ToList();
                return messages;
            }
            catch
            {
                return null;
            }
        }

        //public async Task<bool> ValidateUsername(string username)
        //{
        //    try
        //    {
        //        var userlist = (await firebaseClient
        //        .Child("User")
        //        .OnceAsync<SignUp>()).Select(item =>
        //        new SignUp
        //        {
        //            Username = item.Object.Username,

        //        }).ToList();
        //        bool x = false;
        //        foreach (var item in userlist)
        //        {
        //            if (item.Username == username)
        //            {
        //                x = false;
        //            }
        //            else
        //            {
        //                x = true;
        //            }
        //        }

        //        return x;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //}
    }
}
