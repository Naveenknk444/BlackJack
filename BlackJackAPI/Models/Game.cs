using BlackJackGameLogic.Models;

namespace BlackJackAPI.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public Deck Deck { get; set; } = new Deck(); // Ensure Deck is initialized here
        public List<Card> PlayerHand { get; set; } = new();
        public List<Card> DealerHand { get; set; } = new();

        // Additional properties, like status or score, can be added as needed.
    }
}
