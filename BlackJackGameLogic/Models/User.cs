using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackGameLogic.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public int TotalGamesPlayed { get; set; }
        public int TotalWins { get; set; }
        // Additional properties as needed
    }

}
