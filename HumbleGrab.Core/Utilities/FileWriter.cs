namespace HumbleGrab.Core.Utilities;

public class FileWriter : IDisposable
{
    private readonly string BasePath;

    private readonly IDictionary<string, StreamWriter> Files;

    internal FileWriter(string basePath)
    {
        if (!Directory.Exists(basePath))
        {
            throw new InvalidOperationException($"Path {basePath} does not exist");
        }

        Files = new Dictionary<string, StreamWriter>();
        BasePath = basePath;
    }

    private StreamWriter GetFile(string fileName)
    {
        if (!Files.TryGetValue(fileName, out var file))
        {
            var filePath = $@"{BasePath}\{fileName}";
            if (File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            Files[fileName] = file = new StreamWriter(filePath, true);
        }

        return file;
    }

    ~FileWriter()
    {
        Dispose();
    }

    public void Dispose()
    {
        foreach (var file in Files.Values)
        {
            file.Flush();
            file.Close();
            file.Dispose();
        }

        GC.SuppressFinalize(this);
    }

    public void AppendLine(string fileName, string line)
    {
        GetFile(fileName).WriteLine(line);
    }

    public async Task AppendLineAsync(string fileName, string line)
    {
        await GetFile(fileName).WriteLineAsync(line);
    }
}