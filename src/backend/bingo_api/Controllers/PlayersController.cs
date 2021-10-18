using bingo_api.Data;
using bingo_api.Entities;
using bingo_api.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bingo_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly DataContext _context;

        public PlayersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _context.Players
                .Include(p => p.BingoCard)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(Guid id)
        {
            var player = await _context.Players
                .Include(p => p.BingoCard)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return NotFound("Jogador não encontrado");
            }

            return player;
        }

        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(PlayerRequest playerRequest)
        {
            if (await _context.GameSessions.AnyAsync(gs => gs.Id == playerRequest.GameSessionId) is false)
                return NotFound("Sessão de jogo não encontrada");

            var player = new Player(playerRequest.Name, playerRequest.GameSessionId);
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlayer", new { id = player.Id }, player);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(Guid id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
                return NotFound("Jogador não encontrado");

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}