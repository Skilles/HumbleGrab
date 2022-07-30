using System.Text.Json.Serialization;

namespace HumbleGrab.Steam.Models;

public readonly record struct SteamProfile
(
    [property: JsonPropertyName("game_count")]
    int GameCount, 
    [property: JsonPropertyName("games")]
    IEnumerable<SteamGame> Games
);