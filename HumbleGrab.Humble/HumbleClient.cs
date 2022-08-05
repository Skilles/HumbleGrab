using HumbleGrab.Common;
using HumbleGrab.Common.Interfaces;
using HumbleGrab.Humble.Models;
using HumbleGrab.Humble.Options;
using HumbleGrab.Humble.Utilities.Extensions;

namespace HumbleGrab.Humble;

public class HumbleClient : BaseClient<IHumbleOptions>
{
    override protected string BaseUrl => "https://www.humblebundle.com";

    private const string AllKeysEndpoint = "/api/v1/user/order";
    private const string OrdersEndpoint = "/api/v1/orders?all_tpkds=true";
    
    private const string AuthCookieName = "_simpleauth_sess";

    public HumbleClient(IHumbleOptions options) : base(options) { }

    override protected void ReleaseUnmanagedResources()
    {
        base.ReleaseUnmanagedResources();
        Client.Dispose();
        ClientHandler.Dispose();
    }

    public override async Task<IEnumerable<IGame>> FetchGamesAsync()
    {
        var bundles = (await GetGameBundlesAsync()).ToArray();

        return bundles.SelectMany(b => b.Games).Cast<IGame>();
    }

    private async Task<IEnumerable<HumbleGameBundle>> GetGameBundlesAsync()
    {
        var keys = await GetAllGameKeysAsync();

        var endpoints = keys.ToSeparateEndpoints(OrdersEndpoint, 40);

        var bundleTasks = endpoints.Select(GetGameBundlesAsync);

        var bundleCollection = (await Task.WhenAll(bundleTasks))
            .SelectMany(e => e)
            .Where(bundle => bundle.Games.Any());

        return bundleCollection;
    }

    private async Task<IEnumerable<HumbleGameBundle>> GetGameBundlesAsync(string endpoint)
    {
        var response = await GetResponseAsync(endpoint);
        
        var json = await response.Content.ReadAsStringAsync();
        
        return json.ToGameBundles(Options);
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
        
        message.Headers.Add("cookie", $"{AuthCookieName}={Options.AuthToken}");
        
        var result = await Client.SendAsync(message);
        
        result.EnsureSuccessStatusCode();
        
        return result;
    }
}