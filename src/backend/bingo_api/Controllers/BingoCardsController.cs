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
    public class BingoCardsController : ControllerBase
    {
        private readonly DataContext _context;

        public BingoCardsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BingoCard>>> GetBingoCards()
        {
            return await _context.BingoCards
                .Include(bc => bc.NativeNumbers)
                .Include(bc => bc.MarkedNumbers)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BingoCard>> GetBingoCard(Guid id)
        {
            var bingoCard = await _context.BingoCards
                .Include(bc => bc.NativeNumbers)
                .Include(bc => bc.MarkedNumbers)
                .FirstOrDefaultAsync(bc => bc.Id == id);

            if (bingoCard == null)
                return NotFound();

            return bingoCard;
        }

        [HttpPost("{playerId}")]
        public async Task<ActionResult<BingoCard>> GenerateBingoCard([FromRoute] Guid playerId)
        {
            var player = _context.Players.AsNoTracking().FirstOrDefaultAsync(p => p.Id == playerId);
            if (player is null)
                return NotFound("Jogador não encontrado");

            var bingoCard = new BingoCard(playerId);
            bingoCard.FillNativeNumbers();

            _context.BingoCards.Add(bingoCard);

            await _context.SaveChangesAsync();

            return Created("GetBingoCard", "Cartão gerado");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBingoCard(Guid id)
        {
            var bingoCard = await _context.BingoCards.FindAsync(id);
            if (bingoCard == null)
                return NotFound();

            _context.BingoCards.Remove(bingoCard);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}