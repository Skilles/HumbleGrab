namespace HumbleGrab.Humble.Models.GameKey;

public readonly record struct GameBundle
(
    string Name,
    IEnumerable<Game> Games
);