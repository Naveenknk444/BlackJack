using System;
using System.Collections.Generic;
using BlackJackAPI.Api.Services;
using BlackJackAPI.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlackJackAPI.Api.Services
{
    public class GameService : IGameService
    {
        private readonly Dictionary<int, Game> _games = new();
        private readonly Random _random = new();

        public Game StartGame()
        {
            var newGame = new Game
            {
                GameId = _random.Next(1, 10000),
                Deck = new Deck(),
                PlayerHand = new List<Card>(),
                DealerHand = new List<Card>()
            };
            newGame.Deck.Shuffle();

            newGame.PlayerHand.Add(newGame.Deck.DrawCard());
            newGame.PlayerHand.Add(newGame.Deck.DrawCard());
            newGame.DealerHand.Add(newGame.Deck.DrawCard());
            newGame.DealerHand.Add(newGame.Deck.DrawCard());

            _games[newGame.GameId] = newGame;
            return newGame;
        }

        public GameResult EndGame(int gameId)
        {
            if (!_games.ContainsKey(gameId))
                throw new KeyNotFoundException("Game not found");

            var game = _games[gameId];
            var result = new GameResult
            {
                GameId = gameId,
                PlayerScore = CalculateScore(game.PlayerHand),
                DealerScore = CalculateScore(game.DealerHand)
            };

            result.Result = DetermineWinner(result.PlayerScore, result.DealerScore);
            _games.Remove(gameId);
            return result;
        }

        public Card DrawCard(int gameId, int playerId)
        {
            if (!_games.ContainsKey(gameId))
                throw new KeyNotFoundException("Game not found");

            var game = _games[gameId];
            var card = game.Deck.DrawCard();
            game.PlayerHand.Add(card);

            return card;
        }

        public GameResult Stand(int gameId, int playerId)
        {
            if (!_games.ContainsKey(gameId))
                throw new KeyNotFoundException("Game not found");

            var game = _games[gameId];
            DealerTurn(game);

            return new GameResult
            {
                GameId = gameId,
                PlayerScore = CalculateScore(game.PlayerHand),
                DealerScore = CalculateScore(game.DealerHand),
                Result = DetermineWinner(CalculateScore(game.PlayerHand), CalculateScore(game.DealerHand))
            };
        }

        public SplitResult SplitHand(int gameId, int playerId)
        {
            if (!_games.ContainsKey(gameId))
                throw new KeyNotFoundException("Game not found");

            var game = _games[gameId];
            return new SplitResult { /* Details on both hands post-split */ };
        }

        public GameStatus GetGameStatus(int gameId)
        {
            if (!_games.ContainsKey(gameId))
                throw new KeyNotFoundException("Game not found");

            var game = _games[gameId];
            return new GameStatus
            {
                GameId = gameId,
                PlayerHand = game.PlayerHand,
                DealerHand = game.DealerHand,
                RemainingCards = game.Deck.Cards.Count
            };
        }

        public OddsResult CalculateOdds(int gameId)
        {
            if (!_games.ContainsKey(gameId))
                throw new KeyNotFoundException("Game not found");

            var game = _games[gameId];
            return new OddsResult { /* Probability data */ };
        }

        // Helper methods
        private int CalculateScore(List<Card> hand)
        {
            return 0; // Placeholder
        }

        private void DealerTurn(Game game)
        {
        }

        private string DetermineWinner(int playerScore, int dealerScore)
        {
            return "Player"; // Placeholder
        }
    }
}
