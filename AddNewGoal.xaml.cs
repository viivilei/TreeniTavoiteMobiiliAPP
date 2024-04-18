using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using TreeniTavoiteMobiiliAPP.Models;

namespace TreeniTavoiteMobiiliAPP;

public partial class AddNewGoal : ContentPage
{

  

    private int userId;

    public AddNewGoal(int userId) //parametrina tuotu valitun käyttäjän userID
    {
        InitializeComponent();
        this.userId = userId;
        //varmistaa, että on varmasti oikea henkilö jolle lisätään uutta tavoitetta, voi myös laittaa kommentointiin koska tämä lähinnä debuggausta varten
        //DisplayAlert("Valittu käyttäjä", $"Valittu käyttäjä ID: {userId}", "OK");

    }

    async void Button_Clicked(object sender, EventArgs e)
    {
        //hakee käyttäjän syöttämän tiedon tekstikentistä
        string goalName = goalNameEntry.Text;
        string notes = notesEntry.Text;

        // Uusi tavoite-olio

        Goal newGoal = new Goal
        {
            UserId = userId, //arvo tulee mukana tuodusta userID:stä suoraan
            GoalName = goalName,
            Notes = notes,
            Reached = false // Oletusarvoisesti false, koska uutta tavoitetta ei ole vielä saavutettu
        };

        // Lähetetään uusi tavoite backendiin
        var httpClient = new HttpClient();
        var url = "https://treenidbbackend20240415080224.azurewebsites.net/api/goals";
        var json = JsonConvert.SerializeObject(newGoal);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage response;
        try
        {
            response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Onnistui", "Uusi tavoite lisätty onnistuneesti", "OK");
                
                // Navigoidaan takaisin goal-listaukseen
                
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Virhe", "Virhe uuden tavoitteen lisäämisessä", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Virhe", "Virhe: " + ex.Message, "OK");
        }

        


    }
}