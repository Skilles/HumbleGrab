using System.Text.Json.Serialization;
using HumbleGrab.Common.Interfaces;
using HumbleGrab.Humble.Utilities;

namespace HumbleGrab.Humble.Models;

public readonly record struct HumbleGame
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
) : IGame;