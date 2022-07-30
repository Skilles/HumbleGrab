using HumbleGrab.Humble.Options;

namespace HumbleGrab.Humble.Clients;

public class HumbleClientFactory
{
    public IHumbleOptions Options { get; private set; }
    
    public HumbleClientFactory(IHumbleOptions options)
    {
        Options = options;
    }

    public HumbleClient CreateClient()
    {
        if (Options.AutoMode)
        {
            return new AutoHumbleClient(Options);
        }

        return new ManualHumbleClient(Options);
    }
}