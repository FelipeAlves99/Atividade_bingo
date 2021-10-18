using bingo_api.Entities;
using bingo_api.Mapping;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(new PlayerMapping().Configure);
            modelBuilder.Entity<GameSession>(new GameSessionMapping().Configure);
            modelBuilder.Entity<BingoCard>(new BingoCardMapping().Configure);
            modelBuilder.Entity<NativeNumber>(new NativeNumberMapping().Configure);
            modelBuilder.Entity<MarkedNumber>(new MarkedNumberMapping().Configure);
        }
    }
}