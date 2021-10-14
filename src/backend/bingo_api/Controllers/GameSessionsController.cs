using bingo_api.Data;
using bingo_api.Entities;
using bingo_api.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bingo_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameSessionsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly Random _random = new Random();

        public GameSessionsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameSession>>> GetAllGameSessions()
        {
            return await _context.GameSessions
                .AsNoTracking()
                .Include(gs => gs.Players)
                    .ThenInclude(p => p.BingoCard)
                        .ThenInclude(bc => bc.NativeNumbers)
                .Include(gs => gs.Players)
                    .ThenInclude(p => p.BingoCard)
                        .ThenInclude(bc => bc.MarkedNumbers)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameSession>> GetGameSessionById(Guid id)
        {
            var gameSession = await _context.GameSessions
                .AsNoTracking()
                .Include(gs => gs.Players)
                    .ThenInclude(p => p.BingoCard)
                        .ThenInclude(bc => bc.NativeNumbers)
                .Include(gs => gs.Players)
                    .ThenInclude(p => p.BingoCard)
                        .ThenInclude(bc => bc.MarkedNumbers)
                .FirstOrDefaultAsync(gs => gs.Id == id);                

            if (gameSession == null)
                return NotFound();

            return Ok(gameSession);
        }

        [HttpGet("DrawnNumber")]
        public async Task<ActionResult<int>> DrawnGameSessionNumber()
        {
            return Ok(_random.Next(1, 99));
        }

        [HttpPut("{id}/{number}")]
        public async Task<IActionResult> UpdateGameSession([FromRoute] Guid id, [FromRoute] int number)
        {
            var gameSession = await _context.GameSessions
                .AsNoTracking()
                .Include(gs => gs.Players)
                    .ThenInclude(p => p.BingoCard)
                        .ThenInclude(bc => bc.NativeNumbers)
                .Include(gs => gs.Players)
                    .ThenInclude(p => p.BingoCard)
                        .ThenInclude(bc => bc.MarkedNumbers)
                .FirstOrDefaultAsync(gs => gs.Id == id);

            if (gameSession is null)
                return NotFound("Sessão de jogo não encontrada");

            gameSession.UpdateRound();

            foreach (var player in gameSession.Players)
            {
                var bingoCard = player.BingoCard;

                if(bingoCard.HasNativeNumberToMark(number))
                    _context.MarkedNumbers.Add(new MarkedNumber(number, bingoCard.Id));

                if (bingoCard.NativeNumbers.Select(nn => nn.Number)
                    .Except(bingoCard.MarkedNumbers.Select(mn => mn.Number)).Count() == 0)
                {
                    gameSession.UpdateStatus(EGameStatus.Finished);
                    gameSession.SetWinner(player.Id);
                }
                else
                    gameSession.UpdateStatus(EGameStatus.Started);
            }

            _context.Update(gameSession);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameSessionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<GameSession>> GenerateGameSession()
        {
            var gameSession = new GameSession();

            _context.GameSessions.Add(gameSession);
            await _context.SaveChangesAsync();

            return Created("", gameSession);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameSession(Guid id)
        {
            var gameSession = await _context.GameSessions.FindAsync(id);
            if (gameSession == null)
                return NotFound();

            _context.GameSessions.Remove(gameSession);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameSessionExists(Guid id)
        {
            return _context.GameSessions.Any(e => e.Id == id);
        }
    }
}