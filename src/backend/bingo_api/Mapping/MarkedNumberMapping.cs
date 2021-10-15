using bingo_api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.Mapping
{
    public class MarkedNumberMapping : IEntityTypeConfiguration<MarkedNumber>
    {
        public void Configure(EntityTypeBuilder<MarkedNumber> builder)
        {
            builder.ToTable("MarkedNumbers");

            builder.HasKey(mn => mn.Id);

            builder.HasOne(mn => mn.BingoCard)
                .WithMany(bc => bc.MarkedNumbers)
                .HasForeignKey(mn => mn.BingoCardId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}