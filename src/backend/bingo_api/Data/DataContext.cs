using bingo_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Player> Players { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<BingoCard> BingoCards { get; set; }
        public DbSet<NativeNumber> NativeNumbers { get; set; }
        public DbSet<MarkedNumber> MarkedNumbers { get; set; }
    }
}