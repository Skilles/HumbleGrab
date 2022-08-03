namespace HumbleGrabber.Config;

public static class Options
{
    private static IOptions? Instance;

    public static IOptions Get() => Instance ?? throw new InvalidOperationException("Options not initialized");
    
    public static void Init(IOptions options)
    {
        if (Instance != null)
        {
            throw new Exception("Options already initialized");
        }

        Instance = options;
    }
}