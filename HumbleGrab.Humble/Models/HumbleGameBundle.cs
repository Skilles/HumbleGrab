namespace HumbleGrab.Humble.Models;

public readonly record struct HumbleGameBundle
(
    string Name,
    IEnumerable<HumbleGame> Games
);