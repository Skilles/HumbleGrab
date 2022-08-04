using HumbleGrab.Common.Interfaces;
using HumbleGrab.Core.Export;
using HumbleGrab.Humble.Models;

namespace HumbleGrab.Core.Utilities.Extensions;

public static class GameExtensions
{
    public static GameResult ToGame(this IGame game)
    {
        if (game is HumbleGame hGame)
        {
            return new GameResult(hGame.Name, hGame.Key, "Humble", hGame.Key == null ? "Unredeemed" : "Redeemed");
        }

        return new GameResult(game.Name, "", nameof(GameResult).Replace("Game", ""), "");
    }

    public static IEnumerable<GameResult> ToGames(this IEnumerable<IGame> games) => games.Select(ToGame);
}