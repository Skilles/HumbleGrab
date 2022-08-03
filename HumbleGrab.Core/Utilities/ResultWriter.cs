using HumbleGrab.Common;
using HumbleGrabber.Config;

namespace HumbleGrabber.Client;

public class ResultWriter
{
    private readonly FileWriter Writer;

    public ResultWriter(string outputPath)
    {
        Writer = new FileWriter(outputPath);
    }
    
    // private void SaveBundlesToFile(IEnumerable<HumbleGameBundle> bundles)
    // {
    //     foreach (var bundle in bundles)
    //     {
    //         ExportBundle(bundle);
    //         ExportKeys(bundle.Games);
    //     }
    // }
    //
    // private async Task SaveBundlesToFileAsync(IEnumerable<HumbleGameBundle> bundles)
    // {
    //     foreach (var bundle in bundles)
    //     {
    //         await ExportBundleAsync(bundle);
    //         await ExportKeysAsync(bundle.Games);
    //     }
    // }
    //
    // private void ExportBundle(HumbleGameBundle bundle)
    // {
    //     Writer.AppendLine(Options.BundlesFileName,$"---------------- {bundle.Name} ----------------");
    //     foreach (var game in bundle.Games)
    //     {
    //         Writer.AppendLine(Options.BundlesFileName,game.ToString());
    //     }
    // }
    //
    // private async Task ExportBundleAsync(HumbleGameBundle bundle)
    // {
    //     await Writer.AppendLineAsync(Options.BundlesFileName,$"---------------- {bundle.Name} ----------------");
    //     foreach (var game in bundle.Games)
    //     {
    //         await Writer.AppendLineAsync(Options.BundlesFileName,game.ToString());
    //     }
    // }
    //
    // private void ExportKeys(IEnumerable<HumbleGame> games)
    // {
    //     foreach (var game in games)
    //     {
    //         if (game.Key?.Length == 17)
    //         {
    //             Writer.AppendLine(Options.KeysFileName, game.Key);
    //         }
    //     }
    // }
    //
    // private async Task ExportKeysAsync(IEnumerable<HumbleGame> games)
    // {
    //     foreach (var game in games)
    //     {
    //         if (game.Key?.Length == 17)
    //         {
    //             await Writer.AppendLineAsync(Options.KeysFileName, game.Key);
    //         }
    //     }
    // }
}