using HumbleGrab.Common.Interfaces;
using HumbleGrab.Humble.Models;

namespace HumbleGrab.Humble.Options;

public interface IHumbleOptions : IClientOptions
{
    IEnumerable<GamePlatform> AllowedTypes { get; }

    bool AllowExpired { get; }
}