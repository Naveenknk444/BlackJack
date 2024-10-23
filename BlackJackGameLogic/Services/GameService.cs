using BlackJackGameLogic.Interfaces;
using BlackJackGameLogic.Models;

namespace BlackJackGameLogic.Services
{
    public class GameService : IGameService
    {
        private readonly IGameSessionRepository _gameSessionRepository;

        public GameService(IGameSessionRepository gameSessionRepository)
        {
            _gameSessionRepository = gameSessionRepository;
        }

        public void StartGame(int userId)
        {
            // Logic to start a new game session
            var session = new GameSession
            {
                UserId = userId,
                StartTime = DateTime.UtcNow,
                Cards = new List<Card>(),
                IsCompleted = false
            };
            _gameSessionRepository.Add(session);
        }

        public void PlayerAction(int sessionId, PlayerAction action)
        {
            // Logic to process player actions (hit, stand, etc.)
        }
    }

}
