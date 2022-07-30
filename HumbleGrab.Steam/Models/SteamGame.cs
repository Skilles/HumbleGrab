using System.Text.Json.Serialization;

namespace HumbleGrab.Steam.Models;

public readonly record struct SteamGame
(
    [property: JsonPropertyName("appid")]
    int AppId,
    [property: JsonPropertyName("playtime_forever")]
    int Playtime
);