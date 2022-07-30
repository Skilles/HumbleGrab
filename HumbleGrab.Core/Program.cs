using Config.Net;
using HumbleGrab.Clients;
using HumbleGrab.Options;

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