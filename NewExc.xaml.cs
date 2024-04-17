using Newtonsoft.Json;
using System.Diagnostics;
using TreeniTavoiteMobiiliAPP.Models;

namespace TreeniTavoiteMobiiliAPP;

public partial class NewExc : ContentPage
{
    private int userId;
    private int goalId;

    public NewExc(int userId, int goalId)
    {
        InitializeComponent();

        this.userId = userId;
        this.goalId = goalId;

        // N‰yt‰ parametrit DisplayAlertilla
        ShowParameters();
        GetGoalName();
    }

    private async void ShowParameters()
    {
        // N‰yt‰ parametrit k‰ytt‰j‰lle DisplayAlertilla
        await DisplayAlert("Parametrit", $"userId: {userId}, goalId: {goalId}", "OK");
    }

    async void Button_Clicked(object sender, EventArgs e)
    {
        Exercise newExercise = new Exercise
        {
            UserId = userId,
            GoalId = goalId,
            ExName = exerciseNameEntry.Text,
            Date = DateTime.Now,
            Notes = notesEditor.Text
        };

        var httpClient = new HttpClient();
        var url = "https://treenidbbackend20240415080224.azurewebsites.net/api/exercises";
        var json = JsonConvert.SerializeObject(newExercise);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Onnistui", "Uusi harjoitus lis‰tty onnistuneesti", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            string responseContent = await response.Content.ReadAsStringAsync();

            await DisplayAlert("Virhe", $"Virhe uuden harjoituksen lis‰‰misess‰: {responseContent}", "OK");
        }

    }

    async void Button_Clicked_1(object sender, EventArgs e)
    {

        await DisplayAlert("Parametrit", $"userId: {userId}, goalId: {goalId}", "OK");

        try
        {
            // Luo HttpClient-olio
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://treenidbbackend20240415080224.azurewebsites.net/");

            // Hae harjoitukset backendist‰
            string json = await client.GetStringAsync($"api/exercises/ByUserAndGoal?userId={userId}&goalId={goalId}");

            // Deserialisoi JSON-data Exercise-olioiksi
            List<Exercise> exercises = JsonConvert.DeserializeObject<List<Exercise>>(json);

            // Luo lista harjoitusten nimist‰
            List<string> harjoitukset = exercises.Select(ex => ex.ExName).ToList();

            // N‰yt‰ toimintolista (pop-up-ikkuna) harjoituksilla
            string valittuHarjoitus = await DisplayActionSheet("Valitse harjoitus", "Peruuta", null, harjoitukset.ToArray());

            // T‰ss‰ voit k‰sitell‰ valitun harjoituksen
            if (valittuHarjoitus != null && valittuHarjoitus != "Peruuta")
            {
                // Etsi valittu harjoitus Exercise-listasta
                Exercise selectedExercise = exercises.FirstOrDefault(ex => ex.ExName == valittuHarjoitus);
                if (selectedExercise != null)
                {
                    // N‰yt‰ valitun harjoituksen tiedot k‰ytt‰j‰lle
                    string message = $"Harjoituksen nimi: {selectedExercise.ExName}\n";
                    message += $"P‰iv‰m‰‰r‰: {selectedExercise.Date?.ToString("dd.MM.yyyy")}\n";
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

    private async Task GetGoalName()
    {
        try
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://treenidbbackend20240415080224.azurewebsites.net/");
            string json = await client.GetStringAsync($"api/goals/{goalId}"); // Hae tavoitteen tiedot goalId:n perusteella
            Goal goal = JsonConvert.DeserializeObject<Goal>(json);

            // N‰yt‰ tavoitteen nimi labelissa
            goalLabel.Text = $"Tavoitteesi on: {goal.GoalName}";
        }
        catch (Exception e)
        {
            await DisplayAlert("Virhe", e.Message.ToString(), "OK");
        }
    }
}