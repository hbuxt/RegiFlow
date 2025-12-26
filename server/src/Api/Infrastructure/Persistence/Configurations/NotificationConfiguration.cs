using Api.Domain.Entities;
using Api.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Persistence.Configurations
{
    internal sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public NotificationConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(n => n.Id);

            builder.HasDiscriminator<NotificationType>(nameof(Notification.Type))
                .HasValue<Notification>(NotificationType.Information)
                .HasValue<Invitation>(NotificationType.Invitation);

            builder.Property(n => n.Id)
                .HasColumnOrder(0);

            builder.Property(n => n.RecipientId)
                .HasColumnOrder(1);

            builder.Property(n => n.Type)
                .HasColumnOrder(4)
                .IsRequired();

            builder.Property(n => n.Status)
                .HasColumnOrder(5)
                .IsRequired();

            builder.Property(n => n.Content)
                .HasColumnOrder(6)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(n => n.CreatedAt)
                .HasColumnOrder(8)
                .IsRequired();

            builder.HasOne(n => n.Recipient)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.RecipientId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}