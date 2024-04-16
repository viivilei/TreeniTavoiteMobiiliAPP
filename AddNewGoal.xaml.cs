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
        //varmistaa, ett� on varmasti oikea henkil� jolle lis�t��n uutta tavoitetta, voi my�s laittaa kommentointiin koska t�m� l�hinn� debuggausta varten
        DisplayAlert("Valittu k�ytt�j�", $"Valittu k�ytt�j� ID: {userId}", "OK");

    }

    async void Button_Clicked(object sender, EventArgs e)
    {
        //hakee tiedon tekstikentist�
        string goalName = goalNameEntry.Text;
        string notes = notesEntry.Text;

        // T�ss� rakentuu uusi tavoite-olio

        Goal newGoal = new Goal
        {
            UserId = userId,
            GoalName = goalName,
            Notes = notes,
            Reached = false // Oletusarvoisesti false, koska uusi Goal ei ole saavutettu viel�
        };

        // L�hetet��n uusi Goal backendiin
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
                await DisplayAlert("Onnistui", "Uusi tavoite lis�tty onnistuneesti", "OK");
                
                // Navigoidaan takaisin goal-listaukseen
                
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Virhe", "Virhe uuden tavoitteen lis��misess�", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Virhe", "Virhe: " + ex.Message, "OK");
        }

        


    }
}