using BlackJackGameLogic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackjackDataAccess.Interfaces
{
    public interface IGameSessionRepository
    {
        Task<GameSession> GetSessionById(int id);
        Task<IEnumerable<GameSession>> GetSessionsByUserId(int userId);
        Task AddSession(GameSession session);
        Task UpdateSession(GameSession session);
        Task DeleteSession(int id);
    }
}
