using System;
using System.Timers;
using Xamarin.Forms;

namespace NaijaConnect.Views
{
    public partial class OnboardingPage : ContentPage
    {
        public OnboardingPage()
        {
            InitializeComponent();
        }

        private Timer timer;

        protected override void OnAppearing()
        {
            timer = new Timer(TimeSpan.FromSeconds(5).TotalMilliseconds) { AutoReset = true, Enabled = true };
            timer.Elapsed += Timer_Elapsed;
            base.OnAppearing();


            SignInButton.Opacity = 0;
            SignInButton.Scale = 0.7;
            SignUpButton.Opacity = 0;
            SignUpButton.Scale = 0.7;

            var anime = new Animation();

            anime.Add(0.00, 0.30, new Animation(t => SignInButton.TranslationY = t, 70, 0, Easing.SinIn));
            anime.Add(0.00, 0.30, new Animation(t => SignInButton.Opacity = t, 0, 1, Easing.SinIn));
            anime.Add(0.30, 0.60, new Animation(t => SignUpButton.TranslationY = t, 70, 0, Easing.SinIn));
            anime.Add(0.30, 0.60, new Animation(t => SignUpButton.Opacity = t, 0, 1, Easing.SinIn));
            anime.Add(0.60, 1.00, new Animation(t => SignInButton.Scale = t, 0.7, 1, Easing.SpringOut));
            anime.Add(0.60, 1.00, new Animation(t => SignUpButton.Scale = t, 0.7, 1, Easing.SpringOut));

            anime.Commit(this, "OnboardingPageAnimation", 16, 3000);
        }

        protected override void OnDisappearing()
        {
            timer?.Dispose();
            base.OnDisappearing();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (onboardingWalk.Position == 2)
                {
                    onboardingWalk.Position = 0;

                    return;
                }

                onboardingWalk.Position += 1;

            });
        }
    }
}
