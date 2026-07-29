using Inventory.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.API.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Table Name
            builder.ToTable("Categories");

            // Primary Key
            builder.HasKey(c => c.Id);

            // Name
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            // Description
            builder.Property(c => c.Description)
                   .HasMaxLength(500);

            // IsActive
            builder.Property(c => c.IsActive)
                   .HasDefaultValue(true);

            // CreatedDate
            builder.Property(c => c.CreatedDate)
                   .IsRequired();

            // UpdatedDate
            builder.Property(c => c.UpdatedDate);

            // IsDeleted
            builder.Property(c => c.IsDeleted)
                   .HasDefaultValue(false);

            // Unique Index
            builder.HasIndex(c => c.Name)
                   .IsUnique();
        }
    }
}