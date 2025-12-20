using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Persistence.Configurations
{
    internal sealed class ProjectUserRoleConfiguration : IEntityTypeConfiguration<ProjectUserRole>
    {
        public ProjectUserRoleConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<ProjectUserRole> builder)
        {
            builder.ToTable("ProjectUserRoles");

            builder.HasKey(pur => pur.Id);

            builder.Property(pur => pur.Id)
                .HasColumnOrder(0);

            builder.Property(pur => pur.ProjectUserId)
                .HasColumnOrder(1);

            builder.Property(pur => pur.RoleId)
                .HasColumnOrder(2);

            builder.Property(pur => pur.AssignedAt)
                .HasColumnOrder(3)
                .IsRequired();

            builder.HasOne(pur => pur.ProjectUser)
                .WithMany(pu => pu.ProjectUserRoles)
                .HasForeignKey(pur => pur.ProjectUserId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasOne(pur => pur.Role)
                .WithMany(r => r.ProjectUserRoles)
                .HasForeignKey(pur => pur.RoleId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}