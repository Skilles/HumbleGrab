using System.Text.Json.Serialization;

namespace HumbleGrab.Humble.Models;

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