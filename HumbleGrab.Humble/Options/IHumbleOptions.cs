namespace HumbleGrab.Options;

public interface IHumbleOptions
{
    bool AsyncMode { get; }
    bool AutoMode { get; }
    
    string OutputPath { get; }
    
    string BundlesFileName { get; }
    
    string KeysFileName { get; }
    
    IGameOptions GameOptions { get; }
}
