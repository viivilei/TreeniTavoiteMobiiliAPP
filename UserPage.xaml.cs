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



            //Annetaan latausilmoitus, jos käyttäjien haku backendistä kestää kauan
            user_lataus.Text = "Ladataan käyttäjiä...";

        }

        async void LoadDataFromRestAPI() //hakee käyttäjät backendistä
        {
            try
            {

                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri("https://treenidbbackend20240415080224.azurewebsites.net/");
                string json = await client.GetStringAsync("api/users");

                IEnumerable<User> users = JsonConvert.DeserializeObject<User[]>(json);
                
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

        async void navibutton_Clicked(object sender, EventArgs e)
        {

            User us = (User)userList.SelectedItem;

            if (us == null) //jos yhtään käyttäjää ei ole valittu tulee allaoleva ilmoitus
            {
                await DisplayAlert("Valinta puuttuu", "Valitse työntekijä.", "OK"); 
                                                                                    
            }
            else
            {

                int id = us.UserId;
                //await DisplayAlert("Valittu käyttäjä", $"Valittu käyttäjä ID: {us.UserId}", "OK"); //lähinnä debuggausta varten, varmistetaan että id kulkee mukana
                await Navigation.PushAsync(new GoalPage(id)); // Navigoidaan uudelle sivulle tavoitteen lisäystä varten, mukana valitun käyttäjän userID
            }

        }

        async void lisäysnappi_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddNewUser()); // Navigoidaan uudelle sivulle, jolla voidaan tehdä uuden käyttäjän lisäys
        }
    }
}
