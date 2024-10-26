namespace BlackJackAPI.Models
{
    public class GameSession
    {
        public int GameId { get; set; }
        public Deck Deck { get; private set; }
        public List<Card> PlayerHand { get; private set; }
        public List<Card> DealerHand { get; private set; }
        public bool IsGameActive { get; private set; }

        public GameSession(int gameId)
        {
            GameId = gameId;
            Deck = new Deck();
            PlayerHand = new List<Card>();
            DealerHand = new List<Card>();
            IsGameActive = true;

            StartNewGame();
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

            // Continue drawing cards while the dealer's hand value is less than 17
            while (dealerHandValue < 17 || (dealerHandValue == 17 && HasSoft17(DealerHand)))
            {
                // Draw a card for the dealer
                DealerHand.Add(Deck.DrawCard());

                // Recalculate the dealer's hand value after drawing a card
                dealerHandValue = CalculateHandValue(DealerHand);
            }
        }

        private bool HasSoft17(List<Card> hand)
        {
            // Check if the dealer's hand contains a soft 17
            int value = CalculateHandValue(hand);
            bool hasAce = hand.Any(card => card.Rank == "Ace");
            return value == 17 && hasAce;
        }

        public void EndGame()
        {
            IsGameActive = false;
        }
    }
}
