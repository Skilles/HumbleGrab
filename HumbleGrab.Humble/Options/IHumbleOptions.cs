using HumbleGrab.Common.Interfaces;
using HumbleGrab.Humble.Models;

namespace HumbleGrab.Humble.Options;

public interface IHumbleOptions : IClientOptions
{
    string AuthToken { get; }
    
    IEnumerable<GamePlatform> AllowedTypes { get; }

    bool AllowExpired { get; }
}