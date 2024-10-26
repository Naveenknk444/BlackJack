using System;
using System.Collections.Generic;
using BlackJackAPI.Models;

namespace BlackJackAPI.Api.Services
{
    public class GameService : IGameService
    {
        private readonly Dictionary<int, GameSession> _games = new();
        private readonly Random _random = new();
        private decimal PlayerBalance = 1000; // Initial balance, can be modified as needed


        public Game StartGame()
        {
            int gameId = _random.Next(1, 10000);
            var gameSession = new GameSession(gameId);
            _games[gameId] = gameSession;

            return new Game
            {
                GameId = gameSession.GameId,
                PlayerHand = gameSession.PlayerHand,
                DealerHand = gameSession.DealerHand,
                Deck = gameSession.Deck
            };
        }

        public GameResult EndGame(int gameId)
        {
            if (!_games.ContainsKey(gameId))
                throw new KeyNotFoundException("Game not found");

            var gameSession = _games[gameId];
            var result = DetermineGameResult(gameSession);
            _games.Remove(gameId);
            return result;
        }

        public Card DrawCard(int gameId, int playerId)
        {
            if (!_games.ContainsKey(gameId))
                throw new KeyNotFoundException("Game not found");

            var gameSession = _games[gameId];
            var card = gameSession.Deck.DrawCard();
            gameSession.PlayerHand.Add(card);

            return card;
        }

        public GameResult Stand(int gameId, int playerId)
        {
            if (!_games.ContainsKey(gameId))
                throw new KeyNotFoundException("Game not found");

            var gameSession = _games[gameId];
            gameSession.DealerPlay();
            return DetermineGameResult(gameSession);
        }

        public SplitResult SplitHand(int gameId, int playerId)
        {
            if (!_games.ContainsKey(gameId))
                throw new KeyNotFoundException("Game not found");

            var gameSession = _games[gameId];
            return new SplitResult { /* Details on both hands post-split */ };
        }

        public GameStatus GetGameStatus(int gameId)
        {
            if (!_games.ContainsKey(gameId))
                throw new KeyNotFoundException("Game not found");

            var gameSession = _games[gameId];
            return new GameStatus
            {
                GameId = gameSession.GameId,
                PlayerHand = gameSession.PlayerHand,
                DealerHand = gameSession.DealerHand,
                RemainingCards = gameSession.Deck.Cards.Count
            };
        }

        public OddsResult CalculateOdds(int gameId)
        {
            if (!_games.ContainsKey(gameId))
                throw new KeyNotFoundException("Game not found");

            var gameSession = _games[gameId];
            return new OddsResult { /* Probability data */ };
        }

        // Helper methods

        private int CalculateScore(List<Card> hand)
        {
            int totalValue = 0;
            int aceCount = 0;

            foreach (var card in hand)
            {
                totalValue += card.Value;
                if (card.Rank == "Ace")
                {
                    aceCount++;
                }
            }

            while (totalValue > 21 && aceCount > 0)
            {
                totalValue -= 10; // Adjust Ace value from 11 to 1
                aceCount--;
            }

            return totalValue;
        }

        private bool HasBlackjack(List<Card> hand)
        {
            if (hand.Count == 2)
            {
                bool hasAce = hand.Any(card => card.Rank == "Ace");
                bool hasTenValueCard = hand.Any(card => card.Value == 10);
                return hasAce && hasTenValueCard;
            }
            return false;
        }

        private GameResult DetermineGameResult(GameSession gameSession)
        {
            int playerScore = CalculateScore(gameSession.PlayerHand);
            int dealerScore = CalculateScore(gameSession.DealerHand);

            bool playerHasBlackjack = HasBlackjack(gameSession.PlayerHand);
            bool dealerHasBlackjack = HasBlackjack(gameSession.DealerHand);

            string result;
            decimal payoutMultiplier = 0;

            if (playerHasBlackjack && dealerHasBlackjack)
            {
                result = "Push (Both Player and Dealer have Blackjack)";
                payoutMultiplier = 0; // No payout on a tie
            }
            else if (playerHasBlackjack)
            {
                result = "Player Wins with Blackjack!";
                payoutMultiplier = 1.5M; // 3:2 payout for Blackjack
            }
            else if (dealerHasBlackjack)
            {
                result = "Dealer Wins with Blackjack";
                payoutMultiplier = 0; // No payout for player
            }
            else if (playerScore > 21)
            {
                result = "Player Busts, Dealer Wins";
                payoutMultiplier = 0;
            }
            else if (dealerScore > 21)
            {
                result = "Dealer Busts, Player Wins";
                payoutMultiplier = 1;
            }
            else if (playerScore > dealerScore)
            {
                result = "Player Wins";
                payoutMultiplier = 1;
            }
            else if (playerScore < dealerScore)
            {
                result = "Dealer Wins";
                payoutMultiplier = 0;
            }
            else
            {
                result = "Push (Tie)";
                payoutMultiplier = 0;
            }

            decimal wager = 100; // Example wager amount; adjust based on game settings
            decimal payout = wager * payoutMultiplier;
            PlayerBalance += payout;

            gameSession.EndGame();

            return new GameResult
            {
                GameId = gameSession.GameId,
                PlayerScore = playerScore,
                DealerScore = dealerScore,
                Result = result,
                Payout = payout
            };
        }


    }
}
