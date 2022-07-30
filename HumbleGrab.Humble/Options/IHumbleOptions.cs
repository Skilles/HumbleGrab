using HumbleGrab.Common.Interfaces;

namespace HumbleGrab.Humble.Options;

public interface IHumbleOptions : IClientOptions
{
    string OutputPath { get; }

    string BundlesFileName { get; }

    string KeysFileName { get; }

    IGameOptions GameOptions { get; }
}