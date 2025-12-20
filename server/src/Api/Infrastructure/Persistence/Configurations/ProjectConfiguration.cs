using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Persistence.Configurations
{
    internal sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public ProjectConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnOrder(0);

            builder.Property(p => p.CreatedById)
                .HasColumnOrder(1);
            
            builder.Property(p => p.Name)
                .HasColumnOrder(2)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(p => p.Description)
                .HasColumnOrder(3)
                .IsRequired(false)
                .HasMaxLength(256);

            builder.Property(p => p.CreatedAt)
                .HasColumnOrder(4)
                .IsRequired();

            builder.HasOne(p => p.CreatedBy)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasMany(p => p.ProjectUsers)
                .WithOne(pu => pu.Project)
                .HasForeignKey(pu => pu.ProjectId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}