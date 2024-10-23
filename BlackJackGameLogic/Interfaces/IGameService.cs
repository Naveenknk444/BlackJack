using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJackGameLogic.Models;

namespace BlackJackGameLogic.Interfaces
{
    public interface IGameService
    {
        void StartGame(int userId);
        void PlayerAction(int sessionId, PlayerAction action);
    }

}
