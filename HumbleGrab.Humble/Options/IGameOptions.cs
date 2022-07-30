using HumbleGrab.Humble.Models;

namespace HumbleGrab.Humble.Options;

public interface IGameOptions
{
    IEnumerable<GamePlatform> AllowedTypes { get; }

    bool AllowExpired { get; }
}