using Config.Net;
using HumbleGrab.Humble.Clients;
using HumbleGrab.Humble.Options;

var optionsPath = AppDomain.CurrentDomain.BaseDirectory + @"\config.yaml";

var options = new ConfigurationBuilder<IHumbleOptions>()
    .UseYamlFile(optionsPath)
    .Build();

var factory = new HumbleClientFactory(options);

using var client = factory.CreateClient();

Console.WriteLine("Starting...");
if (options.AsyncMode)
{
    await client.StartAsync();
}
else
{
    client.Start();
}
Console.WriteLine("Finished!");