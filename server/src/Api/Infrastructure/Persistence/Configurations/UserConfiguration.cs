using Api.Domain.Entities;
using Api.Infrastructure.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Persistence.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            
            builder.HasKey(u => u.Id);
            
            builder.Property(u => u.Id)
                .HasColumnOrder(0);

            builder.Property(u => u.FirstName)
                .HasColumnOrder(1)
                .IsRequired(false)
                .HasMaxLength(64);

            builder.Property(u => u.LastName)
                .HasColumnOrder(2)
                .IsRequired(false)
                .HasMaxLength(64);

            builder.Property(u => u.Email)
                .HasColumnOrder(3)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.HashedPassword)
                .HasColumnOrder(4)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(u => u.IsDeleted)
                .HasColumnOrder(5)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .HasColumnOrder(6)
                .IsRequired();

            builder.Property(u => u.DeletedAt)
                .HasColumnOrder(7)
                .IsRequired(false);
            
            builder.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasMany(u => u.Projects)
                .WithOne(p => p.CreatedBy)
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasMany(u => u.ProjectUsers)
                .WithOne(pu => pu.User)
                .HasForeignKey(pu => pu.UserId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasMany(u => u.Notifications)
                .WithOne(n => n.Recipient)
                .HasForeignKey(n => n.RecipientId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasMany(u => u.Invitations)
                .WithOne(i => i.SentBy)
                .HasForeignKey(i => i.SentById)
                .OnDelete(DeleteBehavior.ClientNoAction);
            
            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasData(UserSeeder.Generate());
        }
    }
}