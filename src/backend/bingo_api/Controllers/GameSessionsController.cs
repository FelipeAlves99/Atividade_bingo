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

        public GameSessionsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameSession>>> GetAllGameSessions()
        {
            return await _context.GameSessions.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameSession>> GetGameSessionById(Guid id)
        {
            var gameSession = await _context.GameSessions.FindAsync(id);

            if (gameSession == null)
                return NotFound();

            return Ok(gameSession);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameSession(Guid id, GameSession gameSession)
        {
            if (id != gameSession.Id)
                return BadRequest();

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
        public async Task<ActionResult<GameSession>> PostGameSession(GameSession gameSession)
        {
            _context.GameSessions.Add(gameSession);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Create new game session", new { id = gameSession.Id }, gameSession);
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