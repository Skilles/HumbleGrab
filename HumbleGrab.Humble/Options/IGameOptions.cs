using HumbleGrab.Models.GameKey;

namespace HumbleGrab.Options;

public interface IGameOptions
{
    IEnumerable<GamePlatform> AllowedTypes { get; }
    
    bool AllowExpired { get; }
}