using System.Text.Json;
using System.Text.Json.Nodes;
using HumbleGrab.Humble.Models;
using HumbleGrab.Humble.Options;

namespace HumbleGrab.Humble.Utilities.Extensions;

public static class JsonExtensions
{
    private static HumbleGameBundle ToGameBundle(this JsonNode jsonObject, IGameOptions options)
    {
        var name = jsonObject["product"]!["human_name"]!.GetValue<string>();

        var games = jsonObject["tpkd_dict"]!["all_tpks"]!
            .AsArray()
            .Select(n => n.Deserialize<HumbleGame>())
            .Where(g => options.AllowedTypes.Contains(g.Platform));

        if (!options.AllowExpired)
        {
            games = games.Where(g => !g.IsExpired);
        }

        var gameBundle = new HumbleGameBundle
        {
            Name = name,
            Games = games
        };

        return gameBundle;
    }

    public static IEnumerable<HumbleGameBundle> ToGameBundles(this string json, IGameOptions options)
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