using Xamarin.Forms;

namespace NaijaConnect.Views
{
    public partial class SignInPage : ContentPage
    {
        public SignInPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            TextLabel.Opacity = 1;
            SignUpLabel.Opacity = 1;
            DescriptionLabel.Opacity = 1;
            UsernameEntry.Opacity = 0;
            PasswordEntry.Opacity = 0;
            var signInAnime = new Animation();

            signInAnime.Add(0.00, 0.50, new Animation(t => TextLabel.RotationX = t, 0, 360));
            signInAnime.Add(0.00, 0.50, new Animation(t => SignUpLabel.RotationX = t, 0, 360));
            signInAnime.Add(0.00, 1.00, new Animation(t => DescriptionLabel.RotationX = t, 0, 360));
            signInAnime.Add(0.50, 1.00, new Animation(t => UsernameEntry.TranslationX = t, 70, 0, Easing.SinIn));
            signInAnime.Add(0.50, 1.00, new Animation(t => UsernameEntry.Opacity = t, 0, 1, Easing.SinIn));
            signInAnime.Add(0.50, 1.00, new Animation(t => PasswordEntry.TranslationX = t, -70, 0, Easing.SinIn));
            signInAnime.Add(0.50, 1.00, new Animation(t => PasswordEntry.Opacity = t, 0, 1, Easing.SinIn));

            signInAnime.Commit(this, "SignInAnimation", 16, 3000);
        }

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();

        //    var disappearingAnime = new Animation();

        //    disappearingAnime.Add(0.00, 0.50, new Animation(t => TextLabel.Opacity = t, 1, 0, Easing.SinIn));
        //    disappearingAnime.Add(0.00, 0.50, new Animation(t => SignUpLabel.Opacity = t, 1, 0, Easing.SinIn));
        //    disappearingAnime.Add(0.00, 1.00, new Animation(t => DescriptionLabel.Opacity = t, 1, 0, Easing.SinIn));
        //    disappearingAnime.Add(0.50, 1.00, new Animation(t => UsernameEntry.TranslationX = t, 0, 70, Easing.SinIn));
        //    disappearingAnime.Add(0.50, 1.00, new Animation(t => UsernameEntry.Opacity = t, 1, 0, Easing.SinIn));
        //    disappearingAnime.Add(0.50, 1.00, new Animation(t => PasswordEntry.TranslationX = t, 0, -70, Easing.SinIn));
        //    disappearingAnime.Add(0.50, 1.00, new Animation(t => PasswordEntry.Opacity = t, 1, 0, Easing.SinIn));

        //    disappearingAnime.Commit(this, "SignInDisappearingAnimation", 16, 10000);




        //}
    }
}
