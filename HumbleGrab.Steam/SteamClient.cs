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

    private const string OwnedGamesEndpoint = "/IPlayerService/GetOwnedGames/v1/";

    public SteamClient(ISteamOptions options) : base(options) { }
    
    private async Task<SteamProfile> GetProfileAsync(string steamId)
    {
        var endpoint = $"{OwnedGamesEndpoint}?key={Options.ApiKey}&steamid={steamId}&include_appinfo=true";
        
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

    public override async Task<IEnumerable<IGame>> FetchGamesAsync()
    {
        var profiles = await Task.WhenAll(Options.SteamIds.Select(GetProfileAsync));
        return profiles.SelectMany(p => p.Games).Cast<IGame>();
    }
}