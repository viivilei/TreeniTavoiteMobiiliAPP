using Newtonsoft.Json;
using System.Diagnostics;
using TreeniTavoiteMobiiliAPP.Models;

namespace TreeniTavoiteMobiiliAPP;

public partial class NewExc : ContentPage
{
    private int userId;
    private int goalId;

    public NewExc(int userId, int goalId) //mukana käyttäjän userId ja valitun tavoitteen ID
    {
        InitializeComponent();

        this.userId = userId;
        this.goalId = goalId;

        // Näytä parametrit DisplayAlertilla, debuggausta varten, onko molemmat parametrit vielä mukana matkassa
        //ShowParameters();
        GetGoalName();
    }

    //private async void ShowParameters() 
    //{
        // Näytä parametrit käyttäjälle DisplayAlertilla
        //await DisplayAlert("Parametrit", $"userId: {userId}, goalId: {goalId}", "OK");
    //}

    async void Button_Clicked(object sender, EventArgs e) //Lukee käyttäjän syöttämät tiedot tekstikentistä
    {
        //harjoitus olion luonti
        Exercise newExercise = new Exercise
        {
            UserId = userId, //tulee automaattisesti
            GoalId = goalId, //tulee automaattisesti
            ExName = exerciseNameEntry.Text,
            Date = DateTime.Now, //tulee automaattisesti
            Notes = notesEditor.Text
        };

        //lähetys backendiin
        var httpClient = new HttpClient();
        var url = "https://treenidbbackend20240415080224.azurewebsites.net/api/exercises";
        var json = JsonConvert.SerializeObject(newExercise);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Onnistui", "Uusi harjoitus lisätty onnistuneesti", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            string responseContent = await response.Content.ReadAsStringAsync();

            await DisplayAlert("Virhe", $"Virhe uuden harjoituksen lisäämisessä: {responseContent}", "OK");
        }

    }

    async void Button_Clicked_1(object sender, EventArgs e)
    {

        //await DisplayAlert("Parametrit", $"userId: {userId}, goalId: {goalId}", "OK"); jälleen pelkkä tarkistus

        try
        {
            // Luo HttpClient-olio
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://treenidbbackend20240415080224.azurewebsites.net/");

            // Hae harjoitukset backendistä
            string json = await client.GetStringAsync($"api/exercises/ByUserAndGoal?userId={userId}&goalId={goalId}");

            // Deserialisoi JSON-data Exercise-olioiksi
            List<Exercise> exercises = JsonConvert.DeserializeObject<List<Exercise>>(json);

            // Luo lista harjoitusten nimistä
            List<string> harjoitukset = exercises.Select(ex => ex.ExName).ToList();

            // Näytä toimintolista (pop-up-ikkuna) harjoituksilla
            string valittuHarjoitus = await DisplayActionSheet("Valitse harjoitus", "Peruuta", null, harjoitukset.ToArray());

            // Tässä voit käsitellä valitun harjoituksen
            if (valittuHarjoitus != null && valittuHarjoitus != "Peruuta")
            {
                // Etsi valittu harjoitus Exercise-listasta
                Exercise selectedExercise = exercises.FirstOrDefault(ex => ex.ExName == valittuHarjoitus);
                if (selectedExercise != null)
                {
                    // Näytä valitun harjoituksen tiedot käyttäjälle
                    string message = $"Harjoituksen nimi: {selectedExercise.ExName}\n";
                    message += $"Päivämäärä: {selectedExercise.Date?.ToString("dd.MM.yyyy")}\n";
                    message += $"Muistiinpanot: {selectedExercise.Notes}";

                    await DisplayAlert("Valittu harjoitus", message, "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Virhe", $"Virhe harjoitusten hakemisessa: {ex.Message}", "OK");
        }
    }

    private async Task GetGoalName() //hae tavoitteen nimi goalID:n perusteella selvyyden vuoksi
    {
        try
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://treenidbbackend20240415080224.azurewebsites.net/");
            string json = await client.GetStringAsync($"api/goals/{goalId}"); // Hae tavoitteen tiedot goalId:n perusteella
            Goal goal = JsonConvert.DeserializeObject<Goal>(json);

            // Näytä tavoitteen nimi labelissa
            goalLabel.Text = $"Tavoitteesi on: {goal.GoalName}";
        }
        catch (Exception e)
        {
            await DisplayAlert("Virhe", e.Message.ToString(), "OK");
        }
    }
}