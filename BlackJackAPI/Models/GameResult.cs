namespace BlackJackAPI.Models
{
    public class GameResult
    {
        public int GameId { get; set; }
        public int PlayerScore { get; set; }
        public int DealerScore { get; set; }
        public string Result { get; set; } // Example values: "Player Wins", "Dealer Wins", "Push (Tie)"
        public decimal Payout { get; set; } // Amount awarded to the player based on game outcome
        public decimal UpdatedPlayerBalance { get; internal set; }
    }
}
