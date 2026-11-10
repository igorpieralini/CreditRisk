using Exposure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exposure.Infrastructure.Persistence.Configurations;

public class CreditLimitConfiguration : IEntityTypeConfiguration<CreditLimit>
{
    public void Configure(EntityTypeBuilder<CreditLimit> builder)
    {
        builder.ToTable("credit_limits");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .ValueGeneratedNever();

        builder.Property(l => l.Name)
            .HasMaxLength(180)
            .IsRequired();

        builder.Property(l => l.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(l => l.IsActive)
            .IsRequired();

        builder.Property(l => l.CreatedAt)
            .IsRequired();

        builder.Property(l => l.UpdatedAt)
            .IsRequired();

        builder.HasMany(l => l.Exposures)
            .WithOne(e => e.CreditLimit)
            .HasForeignKey(e => e.CreditLimitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(l => l.Name);
        builder.HasIndex(l => l.IsActive);
    }
}