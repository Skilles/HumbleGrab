using System.Diagnostics;
using System.Reflection;
using HandlebarsDotNet;
using HumbleGrab.Common.Interfaces;
using HumbleGrab.Core.Utilities.Extensions;

namespace HumbleGrab.Core.Export;

public class ResultBuilder
{
    private const string ResultsTemplateFile = "HumbleGrab.Core.Results.hbs";
    
    private readonly HandlebarsTemplate<TextWriter, object, object> ResultsTemplate;

    private readonly string OutputPath;
    
    public ResultBuilder(string outputPath)
    {
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
        
        Handlebars.Configuration.ThrowOnUnresolvedBindingExpression = true;
        
        var templateFile = Assembly.GetExecutingAssembly().GetManifestResourceStream(ResultsTemplateFile) ?? throw new HandlebarsRuntimeException("Template not found");
        
        ResultsTemplate = Handlebars.Compile(new StreamReader(templateFile));
        OutputPath = outputPath;
    }

    private static GameResult[] ToGameResults(IEnumerable<IGame> games) 
        => games
           .ToGames()
           .OrderBy(g => g.Status)
           .ThenBy(g => g.Name)
           .ToArray();

    public async Task ExportAsync(ResultCollection results)
    {
        var games = ToGameResults(results.GetUnmatchedGames());

        var gameGroups = games.ToLookup(g => g.Status);

        var redeemedGames = gameGroups["Redeemed"];
        
        var data = new {
            Date = DateTime.Now.ToString("MM/dd/yyyy"), 
            Tables = new[]
            {
                new {
                    Name = "Redeemed Games",
                    Color = "#7fffd4",
                    TextColor = "#1B1E24",
                    Games = redeemedGames
                },
                new {
                    Name = "Unredeemed Games",
                    Color = "#C77FFF",
                    TextColor = "#1B1E24",
                    Games = gameGroups["Unredeemed"]
                },
                new {
                    Name = "Unknown Games",
                    Color = "#cd5c5c",
                    TextColor = "#1B1E24",
                    Games = gameGroups["Unknown"]
                }
            }};

        await Task.WhenAll(ExportResultsAsync(data, true), 
                           ExportKeysAsync(redeemedGames.Select(g => g.Key)!));
    }

    private async Task ExportResultsAsync(object data, bool openAfter)
    {
        var path = $"{OutputPath}results.html";

        await using TextWriter writer = new StreamWriter(path);
        
        ResultsTemplate(writer, data);
        
        writer.Close();

        if (openAfter)
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(path)
            {
                UseShellExecute = true
            };
            p.Start();
        }
    }

    private async Task ExportKeysAsync(IEnumerable<string> keys)
    {
        var path = $"{OutputPath}keys.txt";

        await using TextWriter writer = new StreamWriter(path);

        await writer.WriteAsync(string.Join("\n", keys));
        
        writer.Close();
    }
}