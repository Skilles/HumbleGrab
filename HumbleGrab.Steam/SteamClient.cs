using System.Text.Json;
using System.Text.Json.Nodes;
using HumbleGrab.Steam.Models;
using HumbleGrab.Steam.Options;

namespace HumbleGrab.Steam;

public class SteamClient : IDisposable
{
    private const string BaseUrl = "https://api.steampowered.com";

    private const string OwnedGamesEndpoint = "/IPlayerService/GetOwnedGames/v0001/";
    
    private readonly HttpClientHandler ClientHandler;
    
    protected readonly HttpClient Client;
    
    protected readonly ISteamOptions Options;
    
    public SteamClient(ISteamOptions options)
    {
        Options = options;
        ClientHandler = new HttpClientHandler { UseCookies = false };
        Client = new HttpClient(ClientHandler) { BaseAddress = new Uri(BaseUrl) };
    }

    public async Task<SteamProfile> GetProfileAsync()
    {
        var endpoint = $"{OwnedGamesEndpoint}?key={Options.ApiKey}&steamid={Options.SteamId}";
        
        var response = await Client.GetAsync(endpoint);
        
        var json = await response.Content.ReadAsStringAsync();

        return ParseProfile(json);
    }
    
    private static SteamProfile ParseProfile(string json)
    {
        var jsonObject = JsonNode.Parse(json)!.AsObject();

        var profile = jsonObject["response"].Deserialize<SteamProfile>();
        
        return profile;
    }

    public void Dispose()
    {
        Client.Dispose();
        ClientHandler.Dispose();
        GC.SuppressFinalize(this);
    }
}