using System.Threading.Tasks.Dataflow;
using HumbleGrab.Common.Interfaces;
using HumbleGrab.Core.Config;
using HumbleGrab.Core.Export;
using HumbleGrab.Core.Utilities;

namespace HumbleGrab.Core.Client;

public class ClientRunner
{
    private readonly IOptions Options;

    private readonly IDictionary<Type, IClient> Clients;

    private readonly ClientFactory ClientFactory;
    
    private readonly ResultBuilder ResultBuilder;

    private bool WriteResults;

    public ClientRunner(IOptions options)
    {
        Options = options;
        ClientFactory = new ClientFactory(options);
        ResultBuilder = new ResultBuilder(Options.OutputPath.Replace("%CURRENT_DIR%", Directory.GetCurrentDirectory()));
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

    private IEnumerable<IGame> GetGamesFromResults(ResultCollection results) 
        => Options.ResultMode switch
          {
              ResultMode.Common => results.GetCommonGames(),
              ResultMode.All => results.GetAllGames(),
              ResultMode.Unredeemed => results.GetUnmatchedGames(),
              _ => new List<IGame>()
          };

    public IEnumerable<IGame> Run()
    {
        var results = new ResultCollection();
        
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

        if (WriteResults)
        {
            ResultBuilder.Export(results);
        }

        return GetGamesFromResults(results);
    }
}