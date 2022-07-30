using System.Text.Json;
using System.Text.Json.Nodes;
using HumbleGrab.Models.GameKey;
using HumbleGrab.Options;

namespace HumbleGrab.Utilities.Extensions;

public static class JsonExtensions
{
    private static GameBundle ToGameBundle(this JsonNode jsonObject, IGameOptions options)
    {
        var name = jsonObject["product"]!["human_name"]!.GetValue<string>();

        var games = jsonObject["tpkd_dict"]!["all_tpks"]!
            .AsArray()
            .Select(n => n.Deserialize<Game>())
            .Where(g => options.AllowedTypes.Contains(g.Platform));
        
        if (!options.AllowExpired)
        {
            games = games.Where(g => !g.IsExpired);
        }
        
        var gameBundle = new GameBundle
        {
            Name = name,
            Games = games
        };

        return gameBundle;
    }

    public static IEnumerable<GameBundle> ToGameBundles(this string json, IGameOptions options)
    {
        var jsonObjects = JsonNode.Parse(json)!.AsObject();
        return jsonObjects
            .Select(e => e.Value!.ToGameBundle(options))
            .Where(b => b.Games.Any());
    }

    public static IEnumerable<string> ToGameKeys(this string json)
    {
        var jsonObjects = JsonNode.Parse(json)!.AsArray();
        return jsonObjects
            .Select(e => e!["gamekey"]!.GetValue<string>());
    }
}