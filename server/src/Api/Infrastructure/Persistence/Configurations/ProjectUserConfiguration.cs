using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Persistence.Configurations
{
    internal sealed class ProjectUserConfiguration : IEntityTypeConfiguration<ProjectUser>
    {
        public ProjectUserConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<ProjectUser> builder)
        {
            builder.ToTable("ProjectUsers");

            builder.HasKey(pu => pu.Id);

            builder.Property(pu => pu.Id)
                .HasColumnOrder(0);

            builder.Property(pu => pu.ProjectId)
                .HasColumnOrder(1);

            builder.Property(pu => pu.UserId)
                .HasColumnOrder(2);

            builder.Property(pu => pu.JoinedAt)
                .HasColumnOrder(3)
                .IsRequired();

            builder.HasOne(pu => pu.Project)
                .WithMany(p => p.ProjectUsers)
                .HasForeignKey(pu => pu.ProjectId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasOne(pu => pu.User)
                .WithMany(u => u.ProjectUsers)
                .HasForeignKey(pu => pu.UserId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasMany(pu => pu.ProjectUserRoles)
                .WithOne(pur => pur.ProjectUser)
                .HasForeignKey(pur => pur.ProjectUserId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}