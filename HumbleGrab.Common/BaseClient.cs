using HumbleGrab.Common.Interfaces;

namespace HumbleGrab.Common;

public abstract class BaseClient<T> : IClient where T : IClientOptions
{
    abstract protected string BaseUrl { get; }

    protected readonly T Options;

    protected readonly HttpClientHandler ClientHandler;

    protected readonly HttpClient Client;

    private bool Disposed;

    protected internal BaseClient(T options)
    {
        ClientHandler = new HttpClientHandler {UseCookies = false};
        Client = new HttpClient(ClientHandler) {BaseAddress = new Uri(BaseUrl)};
        Options = options;
    }

    public abstract IEnumerable<IGame> FetchGames();

    public abstract Task<IEnumerable<IGame>> FetchGamesAsync();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~BaseClient()
    {
        Dispose(false);
    }

    virtual protected void ReleaseUnmanagedResources()
    {
        Client.Dispose();
        ClientHandler.Dispose();
    }

    private void Dispose(bool disposing)
    {
        if (Disposed) return;

        ReleaseUnmanagedResources();

        if (disposing)
        {
            Disposed = true;
        }
    }
}