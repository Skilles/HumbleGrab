using HumbleGrab.Models.GameKey;
using HumbleGrab.Options;
using HumbleGrab.Utilities;

namespace HumbleGrab.Clients;

public abstract class HumbleClient : IDisposable
{
    private readonly FileWriter Writer;

    protected const string AuthCookieValue = "eyJ1c2VyX2lkIjo1MTExMzIxMTQxOTY4ODk2LCJpZCI6InJwYVE5NGNDUkgiLCJhdXRoX3RpbWUiOjE2NTg0MjgyNjZ9|1659117090|86f3489059690f1873a37e9f81c9a40c9d695b8e";
    protected const string AuthCookieName = "_simpleauth_sess";

    protected readonly IHumbleOptions Options;

    private bool Disposed;
    
    protected internal HumbleClient(IHumbleOptions options)
    {
        Writer = new FileWriter(AppDomain.CurrentDomain.BaseDirectory);
        Options = options;
    }
    public abstract void Start();

    public abstract Task StartAsync();

    protected void ExportBundle(GameBundle bundle)
    {
        Writer.AppendLine(Options.BundlesFileName,$"---------------- {bundle.Name} ----------------");
        foreach (var game in bundle.Games)
        {
            Writer.AppendLine(Options.BundlesFileName,game.ToString());
        }
    }

    protected async Task ExportBundleAsync(GameBundle bundle)
    {
        await Writer.AppendLineAsync(Options.BundlesFileName,$"---------------- {bundle.Name} ----------------");
        foreach (var game in bundle.Games)
        {
            await Writer.AppendLineAsync(Options.BundlesFileName,game.ToString());
        }
    }

    protected void ExportKeys(IEnumerable<Game> games)
    {
        foreach (var game in games)
        {
            if (game.Key?.Length == 17)
            {
                Writer.AppendLine(Options.KeysFileName, game.Key);
            }
        }
    }
    
    protected async Task ExportKeysAsync(IEnumerable<Game> games)
    {
        foreach (var game in games)
        {
            if (game.Key?.Length == 17)
            {
                await Writer.AppendLineAsync(Options.KeysFileName, game.Key);
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    ~HumbleClient()
    {
        Dispose(false);
    }
    
    virtual protected void ReleaseUnmanagedResources()
    {
        Writer.Dispose();
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