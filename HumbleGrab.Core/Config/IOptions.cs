using HumbleGrab.Core.Export;
using HumbleGrab.Humble.Options;
using HumbleGrab.Steam.Options;

namespace HumbleGrab.Core.Config;

public interface IOptions
{
    public ResultMode ResultMode { get; }
    
    public string OutputPath { get; }

    public IHumbleOptions HumbleOptions { get; }
    public ISteamOptions SteamOptions { get; }
}