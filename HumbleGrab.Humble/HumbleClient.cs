﻿using HumbleGrab.Common;
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

    private const string AuthCookieValue = "eyJ1c2VyX2lkIjo1MTExMzIxMTQxOTY4ODk2LCJpZCI6InJwYVE5NGNDUkgiLCJhdXRoX3RpbWUiOjE2NTg0MjgyNjZ9|1659117090|86f3489059690f1873a37e9f81c9a40c9d695b8e";
    private const string AuthCookieName = "_simpleauth_sess";

    public HumbleClient(IHumbleOptions options) : base(options) { }

    override protected void ReleaseUnmanagedResources()
    {
        base.ReleaseUnmanagedResources();
        Client.Dispose();
        ClientHandler.Dispose();
    }

    public override IEnumerable<IGame> FetchGames()
    {
        var bundles = GetGameBundlesAsync().GetAwaiter().GetResult().ToArray();
        
        SaveBundlesToFile(bundles);
        
        return bundles.SelectMany(b => b.Games).Cast<IGame>();
    }

    public override async Task<IEnumerable<IGame>> FetchGamesAsync()
    {
        var bundles = (await GetGameBundlesAsync()).ToArray();

        await SaveBundlesToFileAsync(bundles);

        return bundles.SelectMany(b => b.Games).Cast<IGame>();
    }

    private void SaveBundlesToFile(IEnumerable<HumbleGameBundle> bundles)
    {
        foreach (var bundle in bundles)
        {
            ExportBundle(bundle);
            ExportKeys(bundle.Games);
        }
    }

    private async Task SaveBundlesToFileAsync(IEnumerable<HumbleGameBundle> bundles)
    {
        foreach (var bundle in bundles)
        {
            await ExportBundleAsync(bundle);
            await ExportKeysAsync(bundle.Games);
        }
    }
    
    private void ExportBundle(HumbleGameBundle bundle)
    {
        Writer.AppendLine(Options.BundlesFileName,$"---------------- {bundle.Name} ----------------");
        foreach (var game in bundle.Games)
        {
            Writer.AppendLine(Options.BundlesFileName,game.ToString());
        }
    }

    private async Task ExportBundleAsync(HumbleGameBundle bundle)
    {
        await Writer.AppendLineAsync(Options.BundlesFileName,$"---------------- {bundle.Name} ----------------");
        foreach (var game in bundle.Games)
        {
            await Writer.AppendLineAsync(Options.BundlesFileName,game.ToString());
        }
    }

    private void ExportKeys(IEnumerable<HumbleGame> games)
    {
        foreach (var game in games)
        {
            if (game.Key?.Length == 17)
            {
                Writer.AppendLine(Options.KeysFileName, game.Key);
            }
        }
    }
    
    private async Task ExportKeysAsync(IEnumerable<HumbleGame> games)
    {
        foreach (var game in games)
        {
            if (game.Key?.Length == 17)
            {
                await Writer.AppendLineAsync(Options.KeysFileName, game.Key);
            }
        }
    }

    private async Task<IEnumerable<HumbleGameBundle>> GetGameBundlesAsync(IGameOptions options = default)
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