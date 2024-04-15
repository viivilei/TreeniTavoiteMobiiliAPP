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
        DisplayAlert("Valittu k‰ytt‰j‰", $"Valittu k‰ytt‰j‰ ID: {userId}", "OK");

    }

    async void Button_Clicked(object sender, EventArgs e)
    {
        string goalName = goalNameEntry.Text;
        string notes = notesEntry.Text;

        // T‰ss‰ voit tehd‰ logiikan uuden Goalin luomiseksi ja tallentamiseksi

        Goal newGoal = new Goal
        {
            UserId = userId,
            GoalName = goalName,
            Notes = notes,
            Reached = false // Oletusarvoisesti false, koska uusi Goal ei ole saavutettu viel‰
        };

        // L‰hetet‰‰n uusi Goal backendiin
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
                await DisplayAlert("Success", "Uusi Goal lis‰tty onnistuneesti", "OK");
                // Voit lis‰t‰ t‰ss‰ tarvittaessa muita toimintoja, esim. navigoida takaisin edelliselle sivulle
                // Navigoidaan takaisin goal-listaukseen
                
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Virhe uuden Goalin lis‰‰misess‰", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Virhe: " + ex.Message, "OK");
        }

        


    }
}