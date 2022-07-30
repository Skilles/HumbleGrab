using Config.Net;
using HumbleGrab.Models.GameKey;

namespace HumbleGrab.Options;

public class GamePlatformTypeParser : ITypeParser
{
    public bool TryParse(string? value, Type t, out object? result)
    {
        if (Enum.TryParse<GamePlatform>(value, out var platform))
        {
            result = platform;
            return true;
        }
        result = null;
        return false;
    }

    public string? ToRawString(object? value) => value?.ToString()?.ToLower();

    public IEnumerable<Type> SupportedTypes => new[] { typeof(string) };
}