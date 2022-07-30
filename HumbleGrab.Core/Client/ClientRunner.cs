using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;
using HumbleGrab.Common.Interfaces;
using HumbleGrab.Humble;
using HumbleGrabber.Config;

namespace HumbleGrabber.Client;

public class ClientRunner
{
    private readonly IOptions Options;

    private readonly IDictionary<Type, IClient> Clients;

    private readonly IDictionary<IClient, IEnumerable<IGame>> ClientResults;

    private readonly ClientFactory ClientFactory;

    public ClientRunner(IOptions options)
    {
        Options = options;
        ClientFactory = new ClientFactory(options);
        Clients = new Dictionary<Type, IClient>();
        ClientResults = new ConcurrentDictionary<IClient, IEnumerable<IGame>>();
    }

    public ClientRunner AddClient<T>() where T : IClient
    {
        var client = ClientFactory.CreateClient<T>();

        Clients.Add(typeof(T), client);

        return this;
    }

    public IEnumerable<IGame> Run()
    {
        var block = new ActionBlock<IClient>(async client =>
        {
            var results = await client.FetchGamesAsync();
            ClientResults.Add(client, results);
        }, new ExecutionDataflowBlockOptions {MaxDegreeOfParallelism = 6});

        foreach (var client in Clients.Values)
        {
            block.Post(client);
        }

        block.Complete();
        block.Completion.GetAwaiter().GetResult();

        return (Options.ResultMode switch
                {
                    GameResultMode.Common => GetCommonGames(),
                    GameResultMode.All => GetAllGames(),
                    GameResultMode.Unredeemed => GetUnmatchedGames(),
                    _ => new List<IGame>()
                }).ToList();
    }

    private IEnumerable<IGame> GetUnmatchedGames()
    {
        var humbleGames = ClientResults[Clients[typeof(HumbleClient)]];
        return humbleGames.Except(GetCommonGames(), new GameComparer());
    }

    private IEnumerable<IGame> GetCommonGames() => Intersect(ClientResults.Values.ToArray(), new GameComparer());

    private IEnumerable<IGame> GetAllGames() => Union(ClientResults.Values.ToArray(), new GameComparer());

    private static IEnumerable<T> Union<T>(IEnumerable<T>[] sequences, IEqualityComparer<T>? comparer = null) => sequences
        .SelectMany(x => x)
        .Distinct(comparer);

    private static IEnumerable<T> Intersect<T>(IEnumerable<T>[] sequences, IEqualityComparer<T>? comparer = null) => sequences
        .Skip(1)
        .Aggregate(
            new HashSet<T>(sequences.First(), comparer),
            (h, e) =>
            {
                h.IntersectWith(e);
                return h;
            });

    private class GameComparer : IEqualityComparer<IGame>
    {
        public bool Equals(IGame? x, IGame? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.SteamId == 0 || y.SteamId == 0) return false; // TODO compare names for id-less games
            return x.SteamId == y.SteamId;
        }

        public int GetHashCode(IGame obj) => obj.SteamId;
    }
}