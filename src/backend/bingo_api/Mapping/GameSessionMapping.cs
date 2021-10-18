using bingo_api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.Mapping
{
    public class GameSessionMapping : IEntityTypeConfiguration<GameSession>
    {
        public void Configure(EntityTypeBuilder<GameSession> builder)
        {
            builder.ToTable("GameSessions");

            builder.HasKey(gs => gs.Id);
        }
    }
}