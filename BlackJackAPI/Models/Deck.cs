using System;
using System.Collections.Generic;

namespace BlackJackAPI.Models
{
    public class Deck
    {
        public List<Card> Cards { get; private set; }

        public Deck()
        {
            Cards = new List<Card>();
            GenerateDeck();
        }

        private void GenerateDeck()
        {
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    Cards.Add(new Card(suit, rank));
                }
            }
        }

        public void Shuffle()
        {
            var random = new Random();
            for (int i = Cards.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var temp = Cards[i];
                Cards[i] = Cards[j];
                Cards[j] = temp;
            }
        }

        public Card DrawCard()
        {
            if (Cards.Count == 0)
            {
                throw new InvalidOperationException("No cards left in the deck.");
            }

            var card = Cards[0];
            Cards.RemoveAt(0);
            return card;
        }
    }
}
