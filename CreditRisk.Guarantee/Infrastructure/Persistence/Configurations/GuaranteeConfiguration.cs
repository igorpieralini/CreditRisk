using CreditRisk.Guarantee.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GuaranteeEntity = global::CreditRisk.Guarantee.Domain.Entities.Guarantee;

namespace CreditRisk.Guarantee.Infrastructure.Persistence.Configurations;

public class GuaranteeConfiguration : IEntityTypeConfiguration<GuaranteeEntity>
{
    public void Configure(EntityTypeBuilder<GuaranteeEntity> builder)
    {
        builder.ToTable("guarantees");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .ValueGeneratedNever();

        builder.Property(g => g.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(g => g.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(g => g.OriginalValue)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(g => g.CreatedAt)
            .IsRequired();

        builder.Property(g => g.IsActive)
            .IsRequired();

        builder.HasMany(g => g.Valuations)
            .WithOne(v => v.Guarantee)
            .HasForeignKey(v => v.GuaranteeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(g => g.Operations)
            .WithOne(o => o.Guarantee)
            .HasForeignKey(o => o.GuaranteeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(g => g.Type);
        builder.HasIndex(g => g.IsActive);
    }
}