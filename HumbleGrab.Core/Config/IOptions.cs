using HumbleGrab.Humble.Options;
using HumbleGrab.Steam.Options;
using HumbleGrabber.Client;

namespace HumbleGrabber.Config;

public interface IOptions
{
    public GameResultMode ResultMode { get; }
    
    public string OutputFolder { get; }

    public IHumbleOptions HumbleOptions { get; }
    public ISteamOptions SteamOptions { get; }
}