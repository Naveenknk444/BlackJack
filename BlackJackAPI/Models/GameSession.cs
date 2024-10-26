using System.Collections.Generic;

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

        // Additional helper methods as needed
        public void EndGame()
        {
            IsGameActive = false;
        }
    }
}
