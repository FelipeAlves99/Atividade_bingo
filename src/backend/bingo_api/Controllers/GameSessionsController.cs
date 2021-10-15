using bingo_api.Data;
using bingo_api.Entities;
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
                return NotFound("Sessão de jogo não encontrada");

            return Ok(gameSession);
        }

        [HttpGet("DrawnNumber/{id}")]
        public async Task<ActionResult<int>> DrawnGameSessionNumber([FromRoute] string id)
        {
            if (Guid.TryParse(id, out Guid idOut) is false)
                return BadRequest("Id informado é inválido.");

            if (await _context.GameSessions.FindAsync(idOut) is null)
                return NotFound("Sessão de jogo não encontrada.");

            var number = _random.Next(1, 99);

            while (StaticHelpers.oldDrawnNumbers.Contains(new Tuple<Guid, int>(idOut, number)))
                number = _random.Next(1, 99);

            StaticHelpers.oldDrawnNumbers.Add(new Tuple<Guid, int>(idOut, number));

            return Ok(number);
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

            if (gameSession.GameStatus != EGameStatus.Finished)
            {
                gameSession.UpdateRound();

                foreach (var player in gameSession.Players)
                {
                    var bingoCard = player.BingoCard;

                    foreach (var oldNumber in StaticHelpers.oldDrawnNumbers.Where(dn => dn.Item1 == gameSession.Id).Select(dn => dn.Item2))
                    {
                        if (await _context.MarkedNumbers.AnyAsync(mn => mn.Number == oldNumber))
                            continue;

                        if (bingoCard.HasNativeNumberToMark(oldNumber))
                        {
                            _context.MarkedNumbers.Add(new MarkedNumber(oldNumber, bingoCard.Id));

                            if (bingoCard.NativeNumbers.Select(nn => nn.Number)
                                .Except(bingoCard.MarkedNumbers.Select(mn => mn.Number)).Count() == 1)
                            {
                                gameSession.UpdateStatus(EGameStatus.Finished);
                                gameSession.SetWinner(player.Id);
                                StaticHelpers.oldDrawnNumbers.RemoveAll(dn => dn.Item1 == gameSession.Id);
                                break;
                            }
                            else
                                gameSession.UpdateStatus(EGameStatus.Started);
                        }
                    }

                    if (await _context.MarkedNumbers.AnyAsync(mn => mn.Number == number))
                        continue;

                    if (bingoCard.HasNativeNumberToMark(number))
                    {
                        _context.MarkedNumbers.Add(new MarkedNumber(number, bingoCard.Id));

                        if (bingoCard.NativeNumbers.Select(nn => nn.Number)
                            .Except(bingoCard.MarkedNumbers.Select(mn => mn.Number)).Count() == 1)
                        {
                            gameSession.UpdateStatus(EGameStatus.Finished);
                            gameSession.SetWinner(player.Id);
                            StaticHelpers.oldDrawnNumbers.RemoveAll(dn => dn.Item1 == gameSession.Id);
                            break;
                        }
                        else
                            gameSession.UpdateStatus(EGameStatus.Started);
                    }
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
                        return NotFound("Sessão de jogo não encontrada");
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }

            return Ok("Este jogo já foi finalizado");
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

            _context.GameSessions.Remove(gameSession);
            _context.Players.RemoveRange(gameSession.Players);
            await _context.Players
                .Where(p => p.GameSessionId == gameSession.Id)
                .ForEachAsync(p => _context.BingoCards.Remove(p.BingoCard));
            await _context.Players
                .Where(p => p.GameSessionId == gameSession.Id)
                .ForEachAsync(p => _context.NativeNumbers.RemoveRange(p.BingoCard.NativeNumbers));
            await _context.Players
                .Where(p => p.GameSessionId == gameSession.Id)
                .ForEachAsync(p => _context.MarkedNumbers.RemoveRange(p.BingoCard.MarkedNumbers));

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameSessionExists(Guid id)
        {
            return _context.GameSessions.Any(e => e.Id == id);
        }
    }
}