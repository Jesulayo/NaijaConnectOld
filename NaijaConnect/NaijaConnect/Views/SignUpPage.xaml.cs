using Xamarin.Forms;

namespace NaijaConnect.Views
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SignUpButton.Opacity = 0;
            EmailEntry.Opacity = 0;
            UsernameEntry.Opacity = 0;
            PhoneNumberEntry.Opacity = 0;
            UniversityEntry.Opacity = 0;
            DepartmentEntry.Opacity = 0;
            PasswordEntry.Opacity = 0;
            ConfirmPasswordEntry.Opacity = 0;
            SignUpButton.Opacity = 0;
            var signUp = new Animation();

            signUp.Add(0.00, 0.30, new Animation(t => HeadingLabel.RotationX = t, 0, 360));
            signUp.Add(0.00, 0.30, new Animation(t => SignInLabel.RotationX = t, 0, 360));
            signUp.Add(0.20, 0.40, new Animation(t => EmailEntry.TranslationX = t, 70, 0, Easing.SinIn));
            signUp.Add(0.20, 0.40, new Animation(t => EmailEntry.Opacity = t, 0, 1, Easing.SinIn));
            signUp.Add(0.30, 0.50, new Animation(t => UsernameEntry.TranslationX = t, 70, 0, Easing.SinIn));
            signUp.Add(0.30, 0.50, new Animation(t => UsernameEntry.Opacity = t, 0, 1, Easing.SinIn));
            signUp.Add(0.40, 0.60, new Animation(t => PhoneNumberEntry.TranslationX = t, 70, 0, Easing.SinIn));
            signUp.Add(0.40, 0.60, new Animation(t => PhoneNumberEntry.Opacity = t, 0, 1, Easing.SinIn));
            signUp.Add(0.50, 0.70, new Animation(t => UniversityEntry.TranslationX = t, 70, 0, Easing.SinIn));
            signUp.Add(0.50, 0.70, new Animation(t => UniversityEntry.Opacity = t, 0, 1, Easing.SinIn));
            signUp.Add(0.60, 0.80, new Animation(t => DepartmentEntry.TranslationX = t, 70, 0, Easing.SinIn));
            signUp.Add(0.60, 0.80, new Animation(t => DepartmentEntry.Opacity = t, 0, 1, Easing.SinIn));
            signUp.Add(0.70, 0.90, new Animation(t => PasswordEntry.TranslationX = t, 70, 0, Easing.SinIn));
            signUp.Add(0.70, 0.90, new Animation(t => PasswordEntry.Opacity = t, 0, 1, Easing.SinIn));
            signUp.Add(0.80, 1.00, new Animation(t => ConfirmPasswordEntry.TranslationX = t, 70, 0, Easing.SinIn));
            signUp.Add(0.80, 1.00, new Animation(t => ConfirmPasswordEntry.Opacity = t, 0, 1, Easing.SinIn));
            signUp.Add(0.90, 1.00, new Animation(t => SignUpButton.Opacity = t, 0, 1, Easing.SinIn));

            signUp.Commit(this, "SignUpPageAnimation", 16, 5000);
        }
    }
}
