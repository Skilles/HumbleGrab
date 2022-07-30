using HumbleGrab.Humble.Models.GameKey;
using HumbleGrab.Humble.Options;
using HumbleGrab.Humble.Utilities.Extensions;

namespace HumbleGrab.Humble.Clients;

public class AutoHumbleClient : HumbleClient
{
    
    private const string BaseUrl = "https://www.humblebundle.com";

    private const string AllKeysEndpoint = "/api/v1/user/order";
    private const string OrdersEndpoint = "/api/v1/orders?all_tpkds=true";
    
    private readonly HttpClientHandler ClientHandler;

    private readonly HttpClient Client;

    public AutoHumbleClient(IHumbleOptions options) : base(options)
    {
        ClientHandler = new HttpClientHandler { UseCookies = false };
        Client = new HttpClient(ClientHandler) { BaseAddress = new Uri(BaseUrl) };
    }

    override protected void ReleaseUnmanagedResources()
    {
        base.ReleaseUnmanagedResources();
        Client.Dispose();
        ClientHandler.Dispose();
    }

    public override void Start()
    {
        var bundles = GetGameBundlesAsync().GetAwaiter().GetResult();

        SaveBundlesToFile(bundles);
    }

    public override async Task StartAsync()
    {
        var bundles = await GetGameBundlesAsync();

        await SaveBundlesToFileAsync(bundles);
    }

    private void SaveBundlesToFile(IEnumerable<GameBundle> bundles)
    {
        foreach (var bundle in bundles)
        {
            ExportBundle(bundle);
            ExportKeys(bundle.Games);
        }
    }

    private async Task SaveBundlesToFileAsync(IEnumerable<GameBundle> bundles)
    {
        foreach (var bundle in bundles)
        {
            await ExportBundleAsync(bundle);
            await ExportKeysAsync(bundle.Games);
        }
    }

    private async Task<IEnumerable<GameBundle>> GetGameBundlesAsync(IGameOptions options = default)
    {
        var keys = await GetAllGameKeysAsync();

        var endpoints = keys.ToSeparateEndpoints(OrdersEndpoint, 40);

        var bundleTasks = endpoints.Select(GetGameBundlesAsync).ToArray();

        var bundleCollection = (await Task.WhenAll(bundleTasks)).SelectMany(e => e);

        bundleCollection = bundleCollection.Where(bundle => bundle.Games.Any());

        return bundleCollection;
    }

    private async Task<IEnumerable<GameBundle>> GetGameBundlesAsync(string endpoint)
    {
        var response = await GetResponseAsync(endpoint);
        
        var json = await response.Content.ReadAsStringAsync();
        
        return json.ToGameBundles(Options.GameOptions);
    }

    private async Task<IEnumerable<string>> GetAllGameKeysAsync()
    {
        var response = await GetResponseAsync(AllKeysEndpoint);
        
        var json = await response.Content.ReadAsStringAsync();
        
        return json.ToGameKeys();
    }

    private async Task<HttpResponseMessage> GetResponseAsync(string endpoint)
    {
        var message = new HttpRequestMessage(HttpMethod.Get, endpoint);
        
        message.Headers.Add("cookie", $"{AuthCookieName}={AuthCookieValue}");
        
        var result = await Client.SendAsync(message);
        
        result.EnsureSuccessStatusCode();
        
        return result;
    }
}