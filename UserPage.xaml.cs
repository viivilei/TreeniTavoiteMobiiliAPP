using System.Collections.ObjectModel;
using TreeniTavoiteMobiiliAPP.Models;
using Newtonsoft.Json;


namespace TreeniTavoiteMobiiliAPP
{
    public partial class UserPage : ContentPage
    {
        // Muuttujan alustaminen
        ObservableCollection<User> dataa = new ObservableCollection<User>();

        public UserPage()
        {
            InitializeComponent();
            LoadDataFromRestAPI();


            //Annetaan latausilmoitus
            user_lataus.Text = "Ladataan käyttäjiä...";

        }

        async void LoadDataFromRestAPI()
        {
            try
            {

                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri("https://treenidbbackend20240415080224.azurewebsites.net/");
                string json = await client.GetStringAsync("api/users");

                IEnumerable<User> users = JsonConvert.DeserializeObject<User[]>(json);
                // dataa -niminen observableCollection on alustettukin jo ylhäällä päätasolla että hakutoiminto,
                // pääsee siihen käsiksi.
                dataa = new ObservableCollection<User>(users);

                // Asetetaan datat näkyviin xaml tiedostossa olevalle listalle
                userList.ItemsSource = dataa;

                // Tyhjennetään latausilmoitus label
                user_lataus.Text = "";

            }

            catch (Exception e)
            {
                await DisplayAlert("Virhe", e.Message.ToString(), "SELVÄ!");

            }

        }

    }
}
