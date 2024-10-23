using BlackJackGameLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackGameLogic.Interfaces
{
    public interface IGameSessionRepository
    {
        GameSession GetSession(int sessionId);
        void Add(GameSession session);
    }

}
