using Microsoft.AspNetCore.Mvc.RazorPages;
using BlackJackAPI.Models;

namespace BlackJackAPI.Api.Services
{
    public interface IGameService
    {
        Game StartGame();
        GameResult EndGame(int gameId);
        Card DrawCard(int gameId, int playerId);
        GameResult Stand(int gameId, int playerId);
        SplitResult SplitHand(int gameId, int playerId);
        GameStatus GetGameStatus(int gameId);
        OddsResult CalculateOdds(int gameId);
    }
}
