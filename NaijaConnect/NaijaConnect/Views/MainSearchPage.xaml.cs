using Xamarin.Forms;

namespace NaijaConnect.Views
{
    public partial class MainSearchPage : ContentPage
    {
        public MainSearchPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var searchAnime = new Animation();

            searchAnime.Add(0.00, 1.00, new Animation(t => searchBar.TranslationX = t, 100, 0, Easing.SinIn));

            searchAnime.Commit(this, "SearchAnimation", 16, 500);
        }
    }
}
