using System.Collections.Concurrent;
using HumbleGrab.Common.Interfaces;
using HumbleGrab.Humble;

namespace HumbleGrabber.Client;

public class GameResults
{
    private readonly IDictionary<Type, IEnumerable<IGame>> ClientResults;

    public GameResults()
    {
        ClientResults = new ConcurrentDictionary<Type, IEnumerable<IGame>>();
    }
    
    public void AddResult(Type type, IEnumerable<IGame> results)
    {
        ClientResults.Add(type, results);
    }
    
    public IEnumerable<IGame> GetUnmatchedGames()
    {
        var humbleGames = ClientResults[typeof(HumbleClient)];
        return humbleGames.Except(GetCommonGames(), new GameComparer());
    }

    public IEnumerable<IGame> GetCommonGames() => Intersect(ClientResults.Values.ToArray(), new GameComparer());

    public IEnumerable<IGame> GetAllGames() => Union(ClientResults.Values.ToArray(), new GameComparer());

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