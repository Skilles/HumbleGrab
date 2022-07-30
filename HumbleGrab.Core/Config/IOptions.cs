using HumbleGrab.Humble.Options;
using HumbleGrab.Steam.Options;

namespace HumbleGrabber.Config;

public interface IOptions
{
    public IHumbleOptions HumbleOptions { get; }
    public ISteamOptions SteamOptions { get; }
}