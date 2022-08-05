using System.Diagnostics;
using Config.Net;
using HumbleGrab.Core.Client;
using HumbleGrab.Core.Config;
using HumbleGrab.Humble;
using HumbleGrab.Steam;

var optionsPath = AppDomain.CurrentDomain.BaseDirectory + @"config.yaml";

var options = new ConfigurationBuilder<IOptions>()
    .UseYamlFile(optionsPath)
    .Build();

Options.Init(options);

var runner = new ClientRunner(options)
    .AddClient<HumbleClient>()
    .AddClient<SteamClient>()
    .WriteToFile();

var sw = Stopwatch.StartNew();

await runner.Run();

sw.Stop();

Console.WriteLine($"Took {sw.Elapsed.TotalSeconds} seconds");