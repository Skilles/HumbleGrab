using HumbleGrab.Humble.Options;
using HumbleGrab.Steam;
using HumbleGrabber.Config;

namespace HumbleGrab.Humble.Clients;

public class ClientFactory
{
    public IOptions Options { get; private set; }
    
    public ClientFactory(IOptions options)
    {
        Options = options;
    }

    public HumbleClient CreateHumbleClient()
    {
        if (Options.HumbleOptions.AutoMode)
        {
            return new AutoHumbleClient(Options.HumbleOptions);
        }

        return new ManualHumbleClient(Options.HumbleOptions);
    }

    public SteamClient CreateSteamClient() => new(Options.SteamOptions);
}