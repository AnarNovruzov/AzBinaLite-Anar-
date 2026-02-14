using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            // Table name
            builder.ToTable("Cities");

            // Primary key
            builder.HasKey(x => x.Id);

            // Name
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.HasIndex(x => x.Name)
                .IsUnique();

        }
    }
}
