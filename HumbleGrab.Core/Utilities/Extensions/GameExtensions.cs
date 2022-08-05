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
            var status = "Redeemed";
            if (hGame.Key == null)
            {
                status = "Unredeemed";
            } 
            else if (hGame.SteamId == 0)
            {
                status = "Unknown";
            }

            return new GameResult(hGame.Name, hGame.Key, "Humble", status);
        }

        return new GameResult(game.Name, "", nameof(GameResult).Replace("Game", ""), "");
    }

    public static IEnumerable<GameResult> ToGames(this IEnumerable<IGame> games) => games.Select(ToGame);
}