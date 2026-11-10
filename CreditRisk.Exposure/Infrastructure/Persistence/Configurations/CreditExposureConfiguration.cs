using Exposure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exposure.Infrastructure.Persistence.Configurations;

public class CreditExposureConfiguration : IEntityTypeConfiguration<CreditExposure>
{
    public void Configure(EntityTypeBuilder<CreditExposure> builder)
    {
        builder.ToTable("credit_exposures");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.CreditOperationId)
            .IsRequired();

        builder.Property(e => e.CreditLimitId)
            .IsRequired();

        builder.Property(e => e.OriginalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.OutstandingAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        builder.HasOne(e => e.CreditLimit)
            .WithMany(l => l.Exposures)
            .HasForeignKey(e => e.CreditLimitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Movements)
            .WithOne(m => m.CreditExposure)
            .HasForeignKey(m => m.CreditExposureId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.CreditOperationId)
            .IsUnique();

        builder.HasIndex(e => e.CreditLimitId);
        builder.HasIndex(e => e.Status);
    }
}