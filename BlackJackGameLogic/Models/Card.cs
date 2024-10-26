namespace BlackJackGameLogic.Models
{
    public class Card
    {
        // Properties for the suit of the card (e.g., Hearts, Diamonds, etc.)
        public string Suit { get; set; }

        // Properties for the rank of the card (e.g., Two, King, Ace)
        public string Rank { get; set; }

        // Numerical value associated with the card, important for game logic
        public int Value { get; set; }

        // Constructor to initialize a new instance of the Card class with specified properties
        public Card(string suit, string rank, int value)
        {
            Suit = suit;
            Rank = rank;
            Value = value;
        }
    }
}
