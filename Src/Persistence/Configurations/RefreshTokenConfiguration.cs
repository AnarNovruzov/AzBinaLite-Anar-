using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {

        builder.ToTable("RefreshTokens");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(rt => rt.UserId)
            .IsRequired();

        builder.Property(rt => rt.ExpiresAtUtc)
            .IsRequired();

        builder.Property(rt => rt.CreatedAtUtc)
            .IsRequired();

        // FK User ilə, cascade delete
        builder.HasOne(rt => rt.User)
            .WithMany() // optional: User entity-də ICollection<RefreshToken> varsa .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Token üzrə unique index
        builder.HasIndex(rt => rt.Token)
            .IsUnique();

    }
}
