using BlackJackGameLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackGameLogic.Interfaces
{
    public interface IPerformanceMetricService
    {
        void AddMetric(PerformanceMetric metric);
        IEnumerable<PerformanceMetric> GetMetrics(int userId);
    }

}
