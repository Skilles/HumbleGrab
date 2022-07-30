namespace HumbleGrab.Common.Interfaces;

public interface IClient : IDisposable
{
    public IEnumerable<IGame> FetchGames();

    public Task<IEnumerable<IGame>> FetchGamesAsync();
}