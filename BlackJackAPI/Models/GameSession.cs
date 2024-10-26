using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJackAPI.Models
{
    public class GameSession
    {
        public int GameId { get; set; }
        public Deck Deck { get; private set; }
        public List<Card> PlayerHand { get; private set; }
        public List<Card> DealerHand { get; private set; }
        public List<Card>? PlayerHand1 { get; set; } // First hand after split
        public List<Card>? PlayerHand2 { get; set; } // Second hand after split
        public bool IsGameActive { get; private set; }
        public decimal BetAmount { get; private set; }
        public bool HasDoubledDown { get; set; }
        public bool HasSplit { get; set; } // New property to track split status

        public GameSession(int gameId)
        {
            GameId = gameId;
            Deck = new Deck();
            PlayerHand = new List<Card>();
            DealerHand = new List<Card>();
            PlayerHand1 = null;
            PlayerHand2 = null;
            IsGameActive = true;
            HasDoubledDown = false;
            HasSplit = false;
        }

        public void SetBetAmount(decimal amount)
        {
            BetAmount = amount;
        }

        public void DoubleBet()
        {
            BetAmount *= 2;
        }

        private void StartNewGame()
        {
            Deck.Shuffle();
            DealInitialCards();
        }

        private void DealInitialCards()
        {
            // Deal two cards to the player
            PlayerHand.Add(Deck.DrawCard());
            PlayerHand.Add(Deck.DrawCard());

            // Deal two cards to the dealer
            DealerHand.Add(Deck.DrawCard());
            DealerHand.Add(Deck.DrawCard());
        }

        public int CalculateHandValue(List<Card> hand)
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
                totalValue -= 10; // Change Ace value from 11 to 1
                aceCount--;
            }

            return totalValue;
        }

        public void DealerPlay()
        {
            // Calculate the dealer's hand value
            int dealerHandValue = CalculateHandValue(DealerHand);
            Console.WriteLine($"Initial dealer hand value: {dealerHandValue}");
            Console.WriteLine("Dealer's initial hand:");
            PrintHand(DealerHand);

            // Continue drawing cards while the dealer's hand value is less than 17
            while (dealerHandValue < 17 || (dealerHandValue == 17 && HasSoft17(DealerHand)))
            {
                Console.WriteLine("Dealer draws a card...");
                DealerHand.Add(Deck.DrawCard());

                // Recalculate the dealer's hand value after drawing a card
                dealerHandValue = CalculateHandValue(DealerHand);
                Console.WriteLine($"Updated dealer hand value: {dealerHandValue}");
                Console.WriteLine("Dealer's updated hand:");
                PrintHand(DealerHand);
            }

            Console.WriteLine($"Final dealer hand value: {dealerHandValue}");
            Console.WriteLine("Dealer's final hand:");
            PrintHand(DealerHand);
        }

        private bool HasSoft17(List<Card> hand)
        {
            int value = CalculateHandValue(hand);
            bool hasAce = hand.Any(card => card.Rank == "Ace");
            return value == 17 && hasAce;
        }

        private void PrintHand(List<Card> hand)
        {
            foreach (var card in hand)
            {
                Console.WriteLine(card.ToString());
            }
        }

        public void EndGame()
        {
            IsGameActive = false;
        }
    }
}
