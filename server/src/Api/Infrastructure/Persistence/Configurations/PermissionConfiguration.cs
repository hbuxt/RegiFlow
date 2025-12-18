using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Persistence.Configurations
{
    internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public PermissionConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");
            
            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.Id)
                .HasColumnOrder(0);

            builder.Property(p => p.Name)
                .HasColumnOrder(1)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(p => p.Description)
                .HasColumnOrder(2)
                .IsRequired(false)
                .HasMaxLength(256);

            builder.Property(p => p.CreatedAt)
                .HasColumnOrder(3)
                .IsRequired();
        }
    }
}