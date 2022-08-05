namespace HumbleGrab.Common.Interfaces;

public interface IClient : IDisposable
{
    public Task<IEnumerable<IGame>> FetchGamesAsync();
}