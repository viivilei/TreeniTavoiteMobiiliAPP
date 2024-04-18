using Newtonsoft.Json;
using TreeniTavoiteMobiiliAPP.Models;


namespace TreeniTavoiteMobiiliAPP;

public partial class AddNewUser : ContentPage
{
    public AddNewUser()
    {
        InitializeComponent();
    }

    async void Button_Clicked(object sender, EventArgs e)
    {
        // Luetaan tiedot tekstikentistä
        string firstName = FirstNameEntry.Text;
        string lastName = LastNameEntry.Text;
        string email = EmailEntry.Text;

        // Luo uusi käyttäjä-olio
        var user = new User
        {
            Etunimi = firstName,
            Sukunimi = lastName,
            Sahkoposti = email
        };

        // Lähetä uusi käyttäjä backendiin
        var url = "https://treenidbbackend20240415080224.azurewebsites.net/api/users"; 
        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        using (var httpClient = new HttpClient())
        {
            HttpResponseMessage response;
            try
            {
                response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Onnistui", "Uusi käyttäjä lisätty onnistuneesti", "OK");
                    
                    // Navigoidaan takaisin käyttäjälistaukseen
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Virhe", "Virhe uuden käyttäjän lisäämisessä", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", "Virhe: " + ex.Message, "OK");
            }
        }
    }
}