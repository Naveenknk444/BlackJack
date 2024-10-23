using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackGameLogic.Models
{
    public class PerformanceMetric
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime MetricDate { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        // Additional metrics as needed
    }

}
