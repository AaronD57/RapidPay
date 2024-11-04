using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private string _token;

    public ApiClient(string apiBaseUrl)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(apiBaseUrl) };
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        var loginData = new { Username = username, Password = password };
        var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/auth/login", content);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var tokenData = JsonConvert.DeserializeObject<TokenResponse>(responseBody);
        _token = tokenData.Token;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        return _token;
    }

    public async Task<string> GetCardsAsync()
    {
        var response = await _httpClient.GetAsync("/api/cards");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> CreateCardAsync(Card card)
    {
        var content = new StringContent(JsonConvert.SerializeObject(card), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/api/cards", content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    private class TokenResponse
    {
        public string Token { get; set; }
    }

    public class Card
    {
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        // Add other properties as needed
    }
}
