namespace TreeniTavoiteMobiiliAPP
{
    public partial class App : Application
    {
        public App() //sovelluksen aloitussivuksi asetettu käyttäjä-sivu
        {
            InitializeComponent();

            MainPage = new NavigationPage(new UserPage());
        }
    }
}
