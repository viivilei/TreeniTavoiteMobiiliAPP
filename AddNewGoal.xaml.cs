using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using TreeniTavoiteMobiiliAPP.Models;

namespace TreeniTavoiteMobiiliAPP;

public partial class AddNewGoal : ContentPage
{

  

    private int userId;

    public AddNewGoal(int userId) 
    {
        InitializeComponent();
        this.userId = userId;
        //varmistaa, että on varmasti oikea henkilö jolle lisätään uutta tavoitetta, voi myös laittaa kommentointiin koska tämä lähinnä debuggausta varten
        DisplayAlert("Valittu käyttäjä", $"Valittu käyttäjä ID: {userId}", "OK");

    }

    async void Button_Clicked(object sender, EventArgs e)
    {
        //hakee tiedon tekstikentistä
        string goalName = goalNameEntry.Text;
        string notes = notesEntry.Text;

        // Tässä rakentuu uusi tavoite-olio

        Goal newGoal = new Goal
        {
            UserId = userId,
            GoalName = goalName,
            Notes = notes,
            Reached = false // Oletusarvoisesti false, koska uusi Goal ei ole saavutettu vielä
        };

        // Lähetetään uusi Goal backendiin
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