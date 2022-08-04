using HumbleGrab.Common.Interfaces;
using HumbleGrab.Core.Config;
using HumbleGrab.Humble;
using HumbleGrab.Steam;

namespace HumbleGrab.Core.Client;

public class ClientFactory
{
    private readonly IOptions Options;

    public ClientFactory(IOptions options)
    {
        Options = options;
    }

    public IClient CreateClient<T>() where T : IClient
    {
        IClientOptions options = typeof(T) switch
                                 {
                                     { } humbleClient when humbleClient == typeof(HumbleClient) => Options.HumbleOptions,
                                     { } steamClient when steamClient == typeof(SteamClient) => Options.SteamOptions,
                                     _ => throw new InvalidOperationException("Unknown client type")
                                 };
        return CreateClient(typeof(T), options);
    }

    private static IClient CreateClient(Type type, IClientOptions options) => (Activator.CreateInstance(type, options) as IClient)!;
}