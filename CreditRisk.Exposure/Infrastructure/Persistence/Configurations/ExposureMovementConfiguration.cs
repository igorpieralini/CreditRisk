using Exposure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exposure.Infrastructure.Persistence.Configurations;

public class ExposureMovementConfiguration : IEntityTypeConfiguration<ExposureMovement>
{
    public void Configure(EntityTypeBuilder<ExposureMovement> builder)
    {
        builder.ToTable("exposure_movements");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedNever();

        builder.Property(m => m.CreditExposureId)
            .IsRequired();

        builder.Property(m => m.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(m => m.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(m => m.PreviousBalance)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(m => m.CurrentBalance)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.HasOne(m => m.CreditExposure)
            .WithMany(e => e.Movements)
            .HasForeignKey(m => m.CreditExposureId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(m => new
        {
            m.CreditExposureId,
            m.CreatedAt
        });
    }
}