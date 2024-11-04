using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var appSettings = new AppSettings { ApiBaseUrl = "https://localhost:7179" }; // Change to your API base URL
        var apiClient = new ApiClient(appSettings.ApiBaseUrl);

        try
        {
            // Login
            Console.WriteLine("Logging in...");
            var token = await apiClient.LoginAsync("AaronTest", "1234"); // Replace with valid credentials
            Console.WriteLine($"Logged in. Token: {token}");

            // Get Cards
            Console.WriteLine("Fetching cards...");
            var cards = await apiClient.GetCardsAsync();
            Console.WriteLine($"Cards: {cards}");

            // Create a new card
            var newCard = new ApiClient.Card
            {
                CardNumber = "1234567812345678",
                ExpirationDate = "12/24"
            };
            var createdCard = await apiClient.CreateCardAsync(newCard);
            Console.WriteLine($"Created Card: {createdCard}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
