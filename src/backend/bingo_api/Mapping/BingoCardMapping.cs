using bingo_api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.Mapping
{
    public class BingoCardMapping : IEntityTypeConfiguration<BingoCard>
    {
        public void Configure(EntityTypeBuilder<BingoCard> builder)
        {
            builder.ToTable("BingoCards");

            builder.HasKey(bc => bc.Id);

            builder.HasOne(bc => bc.Player)
                .WithOne(p => p.BingoCard)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}