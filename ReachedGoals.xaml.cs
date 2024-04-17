using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using TreeniTavoiteMobiiliAPP.Models;

namespace TreeniTavoiteMobiiliAPP;

public partial class ReachedGoals : ContentPage
{
    private int userId;

    public ReachedGoals(int userId)
    {
        InitializeComponent();
        this.userId = userId;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetUserName(); // Hae käyttäjän etunimi
        await GetGoalList(); // Hae käyttäjän saavutetut tavoitteet
    }

    private async Task GetUserName()
    {
        try
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://treenidbbackend20240415080224.azurewebsites.net/");
            string json = await client.GetStringAsync($"api/users/{userId}"); // Hae käyttäjän tiedot userId:n perusteella
            User user = JsonConvert.DeserializeObject<User>(json);

            // Näytä käyttäjän etunimi labelissa
            nameLabel.Text = $"Onnea {user.Etunimi}, olet jo saavuttanut kaikki allaolevat tavoitteet";
        }
        catch (Exception e)
        {
            await DisplayAlert("Virhe", e.Message.ToString(), "OK");
        }
    }

    private async Task GetGoalList()
    {
        try
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://treenidbbackend20240415080224.azurewebsites.net/");
            string json = await client.GetStringAsync("api/goals");
            IEnumerable<Goal> goals = JsonConvert.DeserializeObject<Goal[]>(json);

            IEnumerable<Goal> userGoals = goals.Where(goal => goal.UserId == userId && goal.Reached);
            ObservableCollection<Goal> userGoalsCollection = new ObservableCollection<Goal>(userGoals);

            goal2List.ItemsSource = userGoalsCollection;
        }
        catch (Exception e)
        {
            await DisplayAlert("Virhe", e.Message.ToString(), "OK");
        }
    }
}
