using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlackjackDataAccess.Interfaces;
using BlackjackDataAccess.Data;
using BlackJackGameLogic.Models;

namespace BlackjackDataAccess.Repositories
{
    public class GameSessionRepository : IGameSessionRepository
    {
        private readonly ApplicationDbContext _context;

        public GameSessionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GameSession> GetSessionById(int id)
        {
            return await _context.GameSessions.FindAsync(id);
        }

        public async Task<IEnumerable<GameSession>> GetSessionsByUserId(int userId)
        {
            return await _context.GameSessions
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task AddSession(GameSession session)
        {
            await _context.GameSessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSession(GameSession session)
        {
            _context.GameSessions.Update(session);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSession(int id)
        {
            var session = await GetSessionById(id);
            if (session != null)
            {
                _context.GameSessions.Remove(session);
                await _context.SaveChangesAsync();
            }
        }
    }
}
