using BlackJackAPI.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace YourApp.Api.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        // Start Game Endpoint
        [HttpPost("start")]
        public IActionResult StartGame()
        {
            var game = _gameService.StartGame();
            return Ok(game); // Returns game details, including game ID
        }

        // End Game Endpoint
        [HttpPost("end")]
        public IActionResult EndGame([FromBody] EndGameRequest request)
        {
            var result = _gameService.EndGame(request.GameId);
            return Ok(result); // Returns end-game results and updates
        }

        // Draw Card Endpoint
        [HttpPost("draw")]
        public IActionResult DrawCard([FromBody] DrawCardRequest request)
        {
            var card = _gameService.DrawCard(request.GameId, request.PlayerId);
            return Ok(card); // Returns drawn card details
        }

        // Player Stand Endpoint
        [HttpPost("stand")]
        public IActionResult Stand([FromBody] StandRequest request)
        {
            var result = _gameService.Stand(request.GameId, request.PlayerId);
            return Ok(result); // Signals the dealer's turn and game progress
        }

        // Split Hand Endpoint (if applicable)
        [HttpPost("split")]
        public IActionResult Split([FromBody] SplitRequest request)
        {
            var splitResult = _gameService.SplitHand(request.GameId, request.PlayerId);
            return Ok(splitResult); // Returns data for both hands post-split
        }

        // Get Game Status Endpoint
        [HttpGet("status")]
        public IActionResult GetGameStatus([FromQuery] int gameId)
        {
            var status = _gameService.GetGameStatus(gameId);
            return Ok(status); // Returns the current state of the game
        }

        // Optional: Get Odds Calculation Endpoint
        [HttpGet("odds")]
        public IActionResult GetOdds([FromQuery] int gameId)
        {
            var odds = _gameService.CalculateOdds(gameId);
            return Ok(odds); // Returns odds for hit, stand, etc.
        }
    }

    // Request DTOs

    public class EndGameRequest
    {
        public int GameId { get; set; }
    }

    public class DrawCardRequest
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
    }

    public class StandRequest
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
    }

    public class SplitRequest
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
    }
}
