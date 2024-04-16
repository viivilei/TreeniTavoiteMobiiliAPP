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
}