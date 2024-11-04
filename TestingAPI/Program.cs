using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json; // Ensure this is included

class Program
{
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        // Step 1: Authenticate and get JWT token
        var token = await Login("your_username", "your_password");

        if (!string.IsNullOrEmpty(token))
        {
            Console.WriteLine($"JWT Token: {token}");

            // Step 2: Use the token to access a protected API
            var result = await GetProtectedResource(token);
            Console.WriteLine($"Protected Resource Response: {result}");
        }
    }

    private static async Task<string> Login(string username, string password)
    {
        var loginUrl = "http://localhost:5000/api/auth/login"; // Update with your login URL
        var json = $"{{\"Username\":\"{username}\",\"Password\":\"{password}\"}}";

        var request = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(loginUrl, request);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            // Assuming your response contains the token in the format { "Token": "your_jwt_token" }
            dynamic result = JsonConvert.DeserializeObject(responseBody); // Use JsonConvert here
            return result.Token;
        }

        Console.WriteLine("Login failed.");
        return null;
    }

    private static async Task<string> GetProtectedResource(string token)
    {
        var resourceUrl = "http://localhost:5000/api/cardmanagement/create"; // Update with your protected API URL
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync(resourceUrl);
        return await response.Content.ReadAsStringAsync();
    }
}
