using HumbleGrab.Humble.Models.GameKey;

namespace HumbleGrab.Humble.Options;

public interface IGameOptions
{
    IEnumerable<GamePlatform> AllowedTypes { get; }
    
    bool AllowExpired { get; }
}