using Api.Domain.Entities;
using Api.Infrastructure.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Persistence.Configurations
{
    internal sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public UserRoleConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");
            
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });
            
            builder.Property(ur => ur.UserId)
                .HasColumnOrder(0);

            builder.Property(ur => ur.RoleId)
                .HasColumnOrder(1);
            
            builder.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasData(UserRoleSeeder.Generate());
        }
    }
}