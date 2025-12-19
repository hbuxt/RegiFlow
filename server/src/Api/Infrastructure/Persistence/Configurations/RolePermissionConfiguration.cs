using Api.Domain.Entities;
using Api.Infrastructure.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Persistence.Configurations
{
    internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public RolePermissionConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("RolePermissions");
            
            builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });
            
            builder.Property(rp => rp.RoleId)
                .HasColumnOrder(0);

            builder.Property(rp => rp.PermissionId)
                .HasColumnOrder(1);
            
            builder.HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(RolePermissionSeeder.Generate());
        }
    }
}