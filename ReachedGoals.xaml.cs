using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using TreeniTavoiteMobiiliAPP.Models;

namespace TreeniTavoiteMobiiliAPP;

public partial class ReachedGoals : ContentPage
{
    private int userId;

    public ReachedGoals(int userId) //Saavutetut tavoitteet haetaan k‰ytt‰j‰ID:n perusteella
    {
        InitializeComponent();
        this.userId = userId;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetUserName(); // Hae k‰ytt‰j‰n etunimi
        await GetGoalList(); // Hae k‰ytt‰j‰n saavutetut tavoitteet
    }

    private async Task GetUserName() //metodi k‰ytt‰j‰n etunimen n‰ytt‰miseksi
    {
        try
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://treenidbbackend20240415080224.azurewebsites.net/");
            string json = await client.GetStringAsync($"api/users/{userId}"); // Hae k‰ytt‰j‰n tiedot userId:n perusteella
            User user = JsonConvert.DeserializeObject<User>(json);

            // N‰yt‰ k‰ytt‰j‰n etunimi labelissa
            nameLabel.Text = $"Onnea {user.Etunimi}, olet jo saavuttanut kaikki allaolevat tavoitteet";
        }
        catch (Exception e)
        {
            await DisplayAlert("Virhe", e.Message.ToString(), "OK");
        }
    }

    private async Task GetGoalList() //Metodi, jolla haetaan back endist‰ n‰kyviin tavoitteet Reached true ja valitulla userID:ll‰
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
