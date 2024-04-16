using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using TreeniTavoiteMobiiliAPP.Models;

namespace TreeniTavoiteMobiiliAPP
{
    public partial class GoalPage : ContentPage
    {
        int eId;

        public GoalPage(int id)
        {
            InitializeComponent();
            eId = id; // Asetetaan id-muuttujan arvo eId-muuttujalle
        }

        // Metodi, joka suoritetaan aina kun sivulle siirryt‰‰n
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await RefreshGoalList(); // P‰ivitet‰‰n tavoitelista
        }

        // Metodi, joka p‰ivitt‰‰ tavoitelistan
        private async Task RefreshGoalList()
        {
            try
            {
                // Luodaan HttpClient-olio
                HttpClient client = new HttpClient();

                // Asetetaan HttpClientin BaseAddress
                client.BaseAddress = new Uri("https://treenidbbackend20240415080224.azurewebsites.net/");

                // Haetaan JSON-data API:sta
                string json = await client.GetStringAsync("api/goals");

                // Deserialisoidaan JSON-data Goal-olioiksi
                IEnumerable<Goal> goals = JsonConvert.DeserializeObject<Goal[]>(json);

                // Suodatetaan goalit k‰ytt‰j‰ ID:n perusteella ja siten, ett‰ niit‰ ei ole viel‰ saavutettu
                IEnumerable<Goal> userGoals = goals.Where(goal => goal.UserId == eId && !goal.Reached);

                // Luodaan uusi ObservableCollection suodatetuista goaleista
                ObservableCollection<Goal> userGoalsCollection = new ObservableCollection<Goal>(userGoals);

                // Asetetaan suodatetut goalit n‰kyviin listalla
                goalList.ItemsSource = userGoalsCollection;

            }
            catch (Exception e)
            {
                // N‰ytet‰‰n virheilmoitus
                await DisplayAlert("Virhe", e.Message.ToString(), "SELVƒ!");
            }
        }

        // Metodi, joka suoritetaan kun "Saavutetut tavoitteet" -nappia painetaan
        async void ReachedGoals_Clicked(object sender, EventArgs e)
        {
            // N‰ytet‰‰n valitun k‰ytt‰j‰n ID
            await DisplayAlert("Valittu k‰ytt‰j‰", $"Valittu k‰ytt‰j‰ ID: {eId}", "OK");
            //await Navigation.PushAsync(new ReachedGoals(eId));
        }

        // Metodi, joka suoritetaan kun "Aseta uusi tavoite" -nappia painetaan
        async void addnew_Clicked(object sender, EventArgs e)
        {
            // N‰ytet‰‰n valitun k‰ytt‰j‰n ID
            await DisplayAlert("Valittu k‰ytt‰j‰", $"Valittu k‰ytt‰j‰ ID: {eId}", "OK");
            await Navigation.PushAsync(new AddNewGoal(eId)); // Navigoidaan uudelle sivulle
        }

        async void SetReached_Clicked(object sender, EventArgs e)
        {
            if (goalList.SelectedItem != null)
            {
                // Haetaan valittu tavoite
                Goal selectedGoal = (Goal)goalList.SelectedItem;

                // N‰yt‰ k‰ytt‰j‰lle valitun tavoitteen tiedot ennen muutosta
                string goalInfo = $"Valittu tavoite:\nNimi: {selectedGoal.GoalName}\nSaavutettu: {(selectedGoal.Reached ? "Kyll‰" : "Ei")}";

                // N‰yt‰ vahvistusviesti k‰ytt‰j‰lle ennen muutoksen tekemist‰
                bool confirm = await DisplayAlert("Vahvista toiminto", goalInfo + "\nHaluatko todella merkit‰ t‰m‰n tavoitteen saavutetuksi?", "Kyll‰", "Peruuta");

                if (confirm)
                {
                    // Merkitse tavoite saavutetuksi palvelimella
                    var httpClient = new HttpClient();
                    var url = $"https://treenidbbackend20240415080224.azurewebsites.net/api/goals/SetReached/{selectedGoal.GoalId}";

                    try
                    {
                        var response = await httpClient.PutAsync(url, null);
                        if (response.IsSuccessStatusCode)
                        {
                            // Onnitteluviesti, kun tavoite on saavutettu
                            await DisplayAlert("Onnittelut!", $"Tavoite '{selectedGoal.GoalName}' on nyt saavutettu!", "OK");

                            // P‰ivit‰ k‰yttˆliittym‰
                            await RefreshGoalList();
                        }
                        else
                        {
                            await DisplayAlert("Virhe", "Tavoitteen merkitseminen saavutetuksi ep‰onnistui", "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Virhe", ex.Message, "OK");
                    }
                }
            }
            else
            {
                await DisplayAlert("Virhe", "Valitse ensin tavoite", "OK");
            }
        }

        async void addexercise_Clicked(object sender, EventArgs e)
        {
            if (goalList.SelectedItem != null)
            {
                // Haetaan valitun tavoitteen tiedot
                Goal selectedGoal = (Goal)goalList.SelectedItem;

                // N‰ytet‰‰n valitun k‰ytt‰j‰n ja tavoitteen ID:t
                await DisplayAlert("Valittu k‰ytt‰j‰ ja tavoite", $"Valittu k‰ytt‰j‰ ID: {eId}\nValittu tavoite ID: {selectedGoal.GoalId}", "OK");

                // Navigoidaan uudelle sivulle ja v‰litet‰‰n parametreina k‰ytt‰j‰n ID ja tavoitteen ID
                await Navigation.PushAsync(new NewExc(eId, selectedGoal.GoalId));
            }
            else
            {
                await DisplayAlert("Virhe", "Valitse ensin tavoite", "OK");
            }
        }
    }
}