using System.Text.Json.Serialization;
using HumbleGrab.Utilities;

namespace HumbleGrab.Models.GameKey;

public readonly record struct Game
(
    [property: JsonPropertyName("human_name")]
    string Name,
    [property: JsonPropertyName("redeemed_key_val")]
    string Key,
    [property: JsonPropertyName("is_expired")]
    bool IsExpired,
    [property: JsonPropertyName("key_type")]
    GamePlatform Platform,
    [property: JsonPropertyName("steam_app_id"), JsonConverter(typeof(NullableIntConverter))]
    int SteamId 
);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GamePlatform
{
    Steam,
    Gog,
    Origin,
    Blizzard,
    Playstation,
    Uplay,
    Generic,
    External_Key
}