using bingo_api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bingo_api.Mapping
{
    public class NativeNumberMapping : IEntityTypeConfiguration<NativeNumber>
    {
        public void Configure(EntityTypeBuilder<NativeNumber> builder)
        {
            builder.ToTable("NativeNumbers");

            builder.HasKey(nn => nn.Id);

            builder.HasOne(nn => nn.BingoCard)
                .WithMany(bc => bc.NativeNumbers)
                .HasForeignKey(nn => nn.BingoCardId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}