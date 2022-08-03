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

    private readonly ClientFactory ClientFactory;
    
    private readonly ResultWriter ResultWriter;

    private bool WriteResults;

    public ClientRunner(IOptions options)
    {
        Options = options;
        ClientFactory = new ClientFactory(options);
        ResultWriter = new ResultWriter(Options.OutputFolder);
        Clients = new Dictionary<Type, IClient>();
    }

    public ClientRunner AddClient<T>() where T : IClient
    {
        var client = ClientFactory.CreateClient<T>();

        Clients.Add(typeof(T), client);

        return this;
    }

    public ClientRunner WriteToFile()
    {
        WriteResults = true;

        return this;
    }

    public IEnumerable<IGame> Run()
    {
        var results = new GameResults();
        
        var block = new ActionBlock<IClient>(async client =>
        {
            var games = await client.FetchGamesAsync();
            results.AddResult(client.GetType(), games);
        }, new ExecutionDataflowBlockOptions {MaxDegreeOfParallelism = 6});

        foreach (var client in Clients.Values)
        {
            block.Post(client);
        }

        block.Complete();
        block.Completion.GetAwaiter().GetResult();

        return (Options.ResultMode switch
                {
                    GameResultMode.Common => results.GetCommonGames(),
                    GameResultMode.All => results.GetAllGames(),
                    GameResultMode.Unredeemed => results.GetUnmatchedGames(),
                    _ => new List<IGame>()
                }).ToList();
    }
}