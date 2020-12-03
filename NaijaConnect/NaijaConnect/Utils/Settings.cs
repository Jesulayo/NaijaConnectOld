using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NaijaConnect.Utils
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string Key = "settings_key";
        private static readonly string KeyDefault = string.Empty;

        #endregion


        public static string KeySettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(Key, KeyDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(Key, value);
            }
        }

        #region Email Constants

        private const string Email = "email";
        private static readonly string EmailDefault = string.Empty;

        #endregion


        public static string EmailSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(Email, EmailDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(Email, value);
            }
        }

        #region Username Constants

        private const string Username = "username";
        private static readonly string UsernameDefault = string.Empty;

        #endregion


        public static string UsernameSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(Username, UsernameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(Username, value);
            }
        }

        #region PhoneNumber Constants

        private const string PhoneNumber = "phoneNumber";
        private static readonly string PhoneNumberDefault = string.Empty;

        #endregion


        public static string PhoneNumberSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(PhoneNumber, PhoneNumberDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(PhoneNumber, value);
            }
        }

        #region University Constants

        private const string University = "university";
        private static readonly string UniversityDefault = string.Empty;

        #endregion


        public static string UniversitySettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(University, UniversityDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(University, value);
            }
        }

        #region Department Constants

        private const string Department = "department";
        private static readonly string DepartmentDefault = string.Empty;

        #endregion


        public static string DepartmentSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(Department, DepartmentDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(Department, value);
            }
        }

        #region Avatar Constants

        private const string Avatar = "avatar";
        private static readonly string AvatarDefault = string.Empty;

        #endregion


        public static string AvatarSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(Avatar, AvatarDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(Avatar, value);
            }
        }

       

        #region Token Constants

        private const string Token = "Token";
        private static readonly string TokenDefault = string.Empty;

        #endregion

        public static string TokenSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(Token, TokenDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(Token, value);
            }
        }


        public static void ClearAllData()
        {
            AppSettings.Clear();
        }

    }
}
