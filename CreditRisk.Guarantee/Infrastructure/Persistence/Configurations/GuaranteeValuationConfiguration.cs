using CreditRisk.Guarantee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditRisk.Guarantee.Infrastructure.Persistence.Configurations;

public class GuaranteeValuationConfiguration : IEntityTypeConfiguration<GuaranteeValuation>
{
    public void Configure(EntityTypeBuilder<GuaranteeValuation> builder)
    {
        builder.ToTable("guarantee_valuations");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .ValueGeneratedNever();

        builder.Property(v => v.GuaranteeId)
            .IsRequired();

        builder.Property(v => v.MarketValue)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(v => v.HaircutPercentage)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(v => v.EligibleValue)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(v => v.ValuationDate)
            .IsRequired();

        builder.HasOne(v => v.Guarantee)
            .WithMany(g => g.Valuations)
            .HasForeignKey(v => v.GuaranteeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(v => new
        {
            v.GuaranteeId,
            v.ValuationDate
        });
    }
}