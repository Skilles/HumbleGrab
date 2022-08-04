using System.Text.Json.Serialization;
using HumbleGrab.Common.Interfaces;

namespace HumbleGrab.Steam.Models;

public readonly record struct SteamGame
(
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("appid")]
    int SteamId,
    [property: JsonPropertyName("playtime_forever")]
    int Playtime
) : IGame;