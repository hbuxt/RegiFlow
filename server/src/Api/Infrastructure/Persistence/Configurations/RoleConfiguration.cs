using Api.Domain.Entities;
using Api.Infrastructure.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Persistence.Configurations
{
    internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            
            builder.HasKey(r => r.Id);
            
            builder.Property(r => r.Id)
                .HasColumnOrder(0);

            builder.Property(r => r.Name)
                .HasColumnOrder(1)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(r => r.Description)
                .HasColumnOrder(2)
                .IsRequired(false)
                .HasMaxLength(256);

            builder.Property(r => r.Scope)
                .HasColumnOrder(3)
                .IsRequired();

            builder.Property(r => r.CreatedAt)
                .HasColumnOrder(4)
                .IsRequired();

            builder.HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasMany(r => r.ProjectUserRoles)
                .WithOne(pur => pur.Role)
                .HasForeignKey(pur => pur.RoleId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasData(RoleSeeder.Generate());
        }
    }
}