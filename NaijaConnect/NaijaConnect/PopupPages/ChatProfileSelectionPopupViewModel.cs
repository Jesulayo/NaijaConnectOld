using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace NaijaConnect.PopupPages
{
    class ChatProfileSelectionPopupViewModel : INotifyPropertyChanged
    {
        private Image selectedProfileImage = new Image();
        public Image SelectedProfileImage
        {
            get { return selectedProfileImage; }
            set { SetProperty(ref selectedProfileImage, value); }
        }


        private string contact;
        public string Contact
        {
            get { return contact; }
            set { SetProperty(ref contact, value); }
        }

        private string selectedUsername;
        public string SelectedUsername
        {
            get { return selectedUsername; }
            set { SetProperty(ref selectedUsername, value); }
        }


        public ChatProfileSelectionPopupViewModel(ImageSource source, string name)
        {
            SelectedProfileImage.Source = source;
            //SelectedProfileImage = "profilepic.jpeg";
            SelectedUsername = name;
            // FileName to contact name; Example: Rita.jpg => Rita
            //var fileName = source.ToString().Remove(0, 6);
            //Contact = fileName.Remove(fileName.Length - 4, 4);
        }



        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
