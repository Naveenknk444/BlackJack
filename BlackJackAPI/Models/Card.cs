namespace BlackJackAPI.Models
{
    public class Card
    {
        public string Suit { get; set; }
        public string Rank { get; set; }
        public int Value { get; set; } // Add this property

        public Card(string suit, string rank)
        {
            Suit = suit;
            Rank = rank;
            Value = GetCardValue(rank); // Initialize Value based on Rank
        }

        private int GetCardValue(string rank)
        {
            return rank switch
            {
                "2" => 2,
                "3" => 3,
                "4" => 4,
                "5" => 5,
                "6" => 6,
                "7" => 7,
                "8" => 8,
                "9" => 9,
                "10" => 10,
                "Jack" => 10,
                "Queen" => 10,
                "King" => 10,
                "Ace" => 11, // Default value for Ace, adjusted later if needed
                _ => 0
            };
        }

        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
    }
}
