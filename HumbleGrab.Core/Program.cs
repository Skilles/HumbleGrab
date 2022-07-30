using Config.Net;
using HumbleGrab.Humble.Clients;
using HumbleGrabber.Config;

var optionsPath = AppDomain.CurrentDomain.BaseDirectory + @"\config.yaml";

var options = new ConfigurationBuilder<IOptions>()
    .UseYamlFile(optionsPath)
    .Build();

var factory = new ClientFactory(options);

using var humbleClient = factory.CreateHumbleClient();
using var steamClient = factory.CreateSteamClient();

Console.WriteLine("Starting Humble client...");
if (options.HumbleOptions.AsyncMode)
{
    await humbleClient.StartAsync();
}
else
{
    humbleClient.Start();
}
Console.WriteLine("Finished scraping humble bundle");
Console.WriteLine("Starting Steam client...");
var profile = await steamClient.GetProfileAsync();
Console.WriteLine("Finished scraping steam");

Console.WriteLine("Comparing Humble against Steam");



Console.WriteLine("Done!");