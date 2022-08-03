using System.Diagnostics;
using Config.Net;
using HumbleGrab.Humble;
using HumbleGrab.Steam;
using HumbleGrabber.Client;
using HumbleGrabber.Config;

var optionsPath = AppDomain.CurrentDomain.BaseDirectory + @"\config.yaml";

var options = new ConfigurationBuilder<IOptions>()
    .UseYamlFile(optionsPath)
    .Build();

Options.Init(options);

var runner = new ClientRunner(options)
    .AddClient<HumbleClient>()
    .AddClient<SteamClient>()
    .WriteToFile();

var sw = Stopwatch.StartNew();

var result = runner.Run();

sw.Stop();

Console.WriteLine($"Found {result.Count()} games in {sw.Elapsed.TotalSeconds} seconds");