using System.Text.Json;
using System.Text.Json.Nodes;
using HumbleGrab.Common;
using HumbleGrab.Common.Interfaces;
using HumbleGrab.Steam.Models;
using HumbleGrab.Steam.Options;

namespace HumbleGrab.Steam;

public class SteamClient : BaseClient<ISteamOptions>
{
    override protected string BaseUrl => "https://api.steampowered.com";

    private const string OwnedGamesEndpoint = "/IPlayerService/GetOwnedGames/v0001/";

    public SteamClient(ISteamOptions options) : base(options) { }

    private async Task<SteamProfile> GetProfileAsync()
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

    public override IEnumerable<IGame> FetchGames()
    {
        var profile = GetProfileAsync().GetAwaiter().GetResult();
        return profile.Games.Cast<IGame>();
    }

    public override async Task<IEnumerable<IGame>> FetchGamesAsync()
    {
        var profile = await GetProfileAsync();
        return profile.Games.Cast<IGame>();
    }
}