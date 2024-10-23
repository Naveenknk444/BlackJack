using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackGameLogic.Models
{
    public class GameSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public List<Card>? Cards { get; set; } // List of cards in the session
        public bool IsCompleted { get; set; }
        // Additional properties as needed
    }

}
