using Newtonsoft.Json;
using System.Collections.ObjectModel;
using TreeniTavoiteMobiiliAPP.Models;

namespace TreeniTavoiteMobiiliAPP;

public partial class GoalPage : ContentPage
{

    int eId;
    public GoalPage(int id)
	{
		InitializeComponent();
        eId = id; // Asetetaan id-muuttujan arvo eId-muuttujalle
        LoadDataFromRestAPI();
    }

    async void LoadDataFromRestAPI()
    {

        await DisplayAlert("Moi", $"P��sit t�nne asti. K�ytt�j� ID: {eId}", "Ok");

        try
        {



            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://treenidbbackend20240415080224.azurewebsites.net/");
            string json = await client.GetStringAsync("api/goals");

            // Deserialisoidaan JSON data Goal-olioiksi
            IEnumerable<Goal> goals = JsonConvert.DeserializeObject<Goal[]>(json);

            // Suodatetaan goalit k�ytt�j� ID:n perusteella
            IEnumerable<Goal> userGoals = goals.Where(goal => goal.UserId == eId);

            // Luodaan uusi ObservableCollection suodatetuista goaleista
            ObservableCollection<Goal> userGoalsCollection = new ObservableCollection<Goal>(userGoals);

            // Asetetaan suodatetut goaleet n�kyviin listalla
            goalList.ItemsSource = userGoalsCollection;

            // Tyhjennet��n latausilmoitus label
            goal_lataus.Text = "";
        }

        catch (Exception e)
        {
            await DisplayAlert("Virhe", e.Message.ToString(), "SELV�!");

        }
    }
}