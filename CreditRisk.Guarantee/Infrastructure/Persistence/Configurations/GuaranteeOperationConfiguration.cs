using CreditRisk.Guarantee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditRisk.Guarantee.Infrastructure.Persistence.Configurations;

public class GuaranteeOperationConfiguration : IEntityTypeConfiguration<GuaranteeOperation>
{
    public void Configure(EntityTypeBuilder<GuaranteeOperation> builder)
    {
        builder.ToTable("guarantee_operations");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedNever();

        builder.Property(o => o.GuaranteeId)
            .IsRequired();

        builder.Property(o => o.CreditOperationId)
            .IsRequired();

        builder.Property(o => o.AllocatedValue)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(o => o.LinkedAt)
            .IsRequired();

        builder.Property(o => o.ReleasedAt)
            .IsRequired(false);

        builder.Property(o => o.IsActive)
            .IsRequired();

        builder.HasOne(o => o.Guarantee)
            .WithMany(g => g.Operations)
            .HasForeignKey(o => o.GuaranteeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(o => o.GuaranteeId);
        builder.HasIndex(o => o.CreditOperationId);
        builder.HasIndex(o => o.IsActive);
    }
}