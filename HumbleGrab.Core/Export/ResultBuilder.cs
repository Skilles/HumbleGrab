using System.Diagnostics;
using System.Reflection;
using HandlebarsDotNet;
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

    public void Export(ResultCollection results)
    {
        // var data = new Result(
        //     Date: DateTime.Now.ToString("MM/dd/yyyy"), 
        //     Tables: new[]
        //     {
        //         new Table(
        //             Name: "Unredeemed Games", 
        //             Color: "#FF7300FF", 
        //             TextColor: "#1B1E24", 
        //             Games: results
        //                 .GetUnmatchedGames()
        //                 .ToGames()
        //                 .OrderBy(g => g.Status)
        //                 .ThenBy(g => g.Name)
        //                 .ToArray()
        //             )
        //     });
        
        var data = new {
            Date = DateTime.Now.ToString("MM/dd/yyyy"), 
            Tables = new[]
            {
                new {
                    Name = "Unredeemed Games", 
                    Color = "#FF7300FF", 
                    TextColor = "#1B1E24", 
                    Games = results
                           .GetUnmatchedGames()
                           .ToGames()
                           .OrderBy(g => g.Status)
                           .ThenBy(g => g.Name)
                           .ToArray()
                }
            }};

        var path = $"{OutputPath}results.html";
        
        using TextWriter writer = new StreamWriter(path);
        
        ResultsTemplate(writer, data);
        
        writer.Close();

        var p = new Process();
        p.StartInfo = new ProcessStartInfo(path)
        { 
            UseShellExecute = true 
        };
        p.Start();
    }
}