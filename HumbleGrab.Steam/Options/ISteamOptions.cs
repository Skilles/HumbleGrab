using HumbleGrab.Common.Interfaces;

namespace HumbleGrab.Steam.Options;

public interface ISteamOptions : IClientOptions
{
    public string ApiKey { get; }

    public string SteamId { get; }
}