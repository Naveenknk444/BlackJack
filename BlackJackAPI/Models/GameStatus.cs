namespace BlackJackAPI.Models
{
    public class GameStatus
    {
        public int GameId { get; set; }
        public List<Card> PlayerHand { get; set; }
        public List<Card> DealerHand { get; set; }
        public int RemainingCards { get; set; }
    }
}
