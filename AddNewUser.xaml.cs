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
        // Luetaan tiedot tekstikentist�
        string firstName = FirstNameEntry.Text;
        string lastName = LastNameEntry.Text;
        string email = EmailEntry.Text;

        // Luo uusi k�ytt�j�-olio
        var user = new User
        {
            Etunimi = firstName,
            Sukunimi = lastName,
            Sahkoposti = email
        };

        // L�het� uusi k�ytt�j� backendiin
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
                    await DisplayAlert("Onnistui", "Uusi k�ytt�j� lis�tty onnistuneesti", "OK");

                    //p�ivit� k�ytt�j�sivu uuden k�ytt�j�n lis��misen j�lkeen
                    var userPage = new UserPage();
                    await Navigation.PushAsync(userPage);
                }
                else
                {
                    await DisplayAlert("Virhe", "Virhe uuden k�ytt�j�n lis��misess�", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", "Virhe: " + ex.Message, "OK");
            }
        }
    }
}