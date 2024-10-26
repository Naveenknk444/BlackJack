using System;
using System.Collections.Generic;
using BlackJackAPI.Models;

namespace BlackJackAPI.Api.Services
{
    public class GameService : IGameService
    {
        private readonly Dictionary<int, GameSession> _games = new();
        private readonly Random _random = new();
        public decimal BetAmount { get; private set; } // Keep as private set

        public void SetBetAmount(decimal amount)
        {
            BetAmount = amount;
        }

        private decimal PlayerBalance = 1000; // Initial balance for the player

        public decimal GetPlayerBalance()
        {
            return PlayerBalance;
        }

        private void AdjustPlayerBalance(decimal amount)
        {
            PlayerBalance += amount; // Adds or subtracts based on the outcome
        }



        public Game StartGame(decimal betAmount)
        {
            int gameId = new Random().Next(1, 10000);
            var gameSession = new GameSession(gameId);

            _games[gameId] = gameSession;

            // Place the initial bet
            PlaceBet(gameId, betAmount);

            // Start the game by dealing initial cards
            gameSession.Deck.Shuffle();
            gameSession.PlayerHand.Add(gameSession.Deck.DrawCard());
            gameSession.PlayerHand.Add(gameSession.Deck.DrawCard());
            gameSession.DealerHand.Add(gameSession.Deck.DrawCard());
            gameSession.DealerHand.Add(gameSession.Deck.DrawCard());

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
        private (int score, bool isBlackjack) CalculateScore(List<Card> hand)
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

            // Adjust for Aces if the total value exceeds 21
            while (totalValue > 21 && aceCount > 0)
            {
                totalValue -= 10; // Adjust Ace value from 11 to 1
                aceCount--;
            }

            // Check for natural Blackjack
            bool isBlackjack = (totalValue == 21 && hand.Count == 2);

            return (totalValue, isBlackjack);
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

        public void PlaceBet(int gameId, decimal amount)
        {
            if (!_games.ContainsKey(gameId))
                throw new KeyNotFoundException("Game not found");

            var gameSession = _games[gameId];

            if (gameSession.IsGameActive)
                throw new InvalidOperationException("Bet cannot be changed once the game has started.");

            if (amount > PlayerBalance)
                throw new InvalidOperationException("Insufficient balance to place this bet.");

            gameSession.SetBetAmount(amount);
            PlayerBalance -= amount; // Deduct bet amount from player balance
        }

        public void DoubleDown(int gameId)
        {
            if (!_games.ContainsKey(gameId))
                throw new KeyNotFoundException("Game not found");

            var gameSession = _games[gameId];

            // Ensure DoubleDown can only be used at the beginning of the player's turn
            if (gameSession.PlayerHand.Count != 2)
                throw new InvalidOperationException("Double down can only be done at the beginning of the player's turn.");

            decimal additionalBet = gameSession.BetAmount;
            if (PlayerBalance < additionalBet)
                throw new InvalidOperationException("Insufficient balance to double down.");

            gameSession.DoubleBet(); // Use DoubleBet to modify BetAmount
            PlayerBalance -= additionalBet;

            // Draw one additional card for the player
            gameSession.PlayerHand.Add(gameSession.Deck.DrawCard());
            gameSession.HasDoubledDown = true;
        }

        public void SplitHand(int gameId)
        {
            if (!_games.ContainsKey(gameId))
                throw new KeyNotFoundException("Game not found");

            var gameSession = _games[gameId];

            // Ensure the player has exactly two cards and they are of the same rank
            if (gameSession.PlayerHand.Count != 2 || gameSession.PlayerHand[0].Rank != gameSession.PlayerHand[1].Rank)
                throw new InvalidOperationException("Split can only be done with a pair of cards.");

            // Check if the player has enough balance to split
            if (PlayerBalance < gameSession.BetAmount)
                throw new InvalidOperationException("Insufficient balance to split.");

            // Deduct the additional bet amount from PlayerBalance for the split hand
            PlayerBalance -= gameSession.BetAmount;

            // Initialize the split hands
            gameSession.PlayerHand1 = new List<Card> { gameSession.PlayerHand[0] };
            gameSession.PlayerHand2 = new List<Card> { gameSession.PlayerHand[1] };

            // Draw an additional card for each hand
            gameSession.PlayerHand1.Add(gameSession.Deck.DrawCard());
            gameSession.PlayerHand2.Add(gameSession.Deck.DrawCard());

            gameSession.HasSplit = true;
        }

        private GameResult DetermineGameResult(GameSession gameSession)
        {
            decimal totalPayout = 0;
            var dealerOutcome = CalculateScore(gameSession.DealerHand);
            int dealerScore = dealerOutcome.Item1;
            bool dealerHasBlackjack = dealerOutcome.Item2;

            // Check if the player has split their hand
            if (gameSession.HasSplit)
            {
                // Calculate result for PlayerHand1
                var (score1, hasBlackjack1) = CalculateScore(gameSession.PlayerHand1);
                var result1 = CalculateHandOutcome(score1, hasBlackjack1, dealerScore, dealerHasBlackjack, gameSession.BetAmount);
                totalPayout += result1.Payout;

                // Calculate result for PlayerHand2
                var (score2, hasBlackjack2) = CalculateScore(gameSession.PlayerHand2);
                var result2 = CalculateHandOutcome(score2, hasBlackjack2, dealerScore, dealerHasBlackjack, gameSession.BetAmount);
                totalPayout += result2.Payout;

                // Apply total payout to PlayerBalance
                AdjustPlayerBalance(totalPayout);

                // Return combined results and total payout for both hands
                return new GameResult
                {
                    GameId = gameSession.GameId,
                    PlayerScore = score1 + score2,
                    DealerScore = dealerScore,
                    Result = $"{result1.Result}, {result2.Result}",
                    Payout = totalPayout,
                    UpdatedPlayerBalance = PlayerBalance
                };
            }
            else
            {
                // Standard single-hand calculation if no split
                var (playerScore, playerHasBlackjack) = CalculateScore(gameSession.PlayerHand);
                var result = CalculateHandOutcome(playerScore, playerHasBlackjack, dealerScore, dealerHasBlackjack, gameSession.BetAmount);

                // Apply payout to PlayerBalance
                AdjustPlayerBalance(result.Payout);

                // Return single hand result
                return new GameResult
                {
                    GameId = gameSession.GameId,
                    PlayerScore = playerScore,
                    DealerScore = dealerScore,
                    Result = result.Result,
                    Payout = result.Payout,
                    UpdatedPlayerBalance = PlayerBalance
                };
            }
        }

        private GameResult CalculateHandOutcome(int playerScore, bool playerHasBlackjack, int dealerScore, bool dealerHasBlackjack, decimal betAmount)
        {
            string result;
            decimal payoutMultiplier = 0;

            if (playerHasBlackjack && dealerHasBlackjack)
            {
                result = "Push (Both Player and Dealer have Blackjack)";
                payoutMultiplier = 0;
                AdjustPlayerBalance(betAmount); // Return initial bet on tie
            }
            else if (playerHasBlackjack)
            {
                result = "Player Wins with Blackjack!";
                payoutMultiplier = 1.5M;
            }
            else if (dealerHasBlackjack)
            {
                result = "Dealer Wins with Blackjack";
                payoutMultiplier = 0;
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
                AdjustPlayerBalance(betAmount); // Return initial bet on tie
            }

            decimal payout = betAmount * payoutMultiplier;
            AdjustPlayerBalance(payout);

            return new GameResult
            {
                GameId = 0,
                PlayerScore = playerScore,
                DealerScore = dealerScore,
                Result = result,
                Payout = payout
            };
        }


    }
}
