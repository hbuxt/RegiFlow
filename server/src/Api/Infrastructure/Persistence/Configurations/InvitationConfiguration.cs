using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Persistence.Configurations
{
    internal class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
    {
        public InvitationConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<Invitation> builder)
        {
            builder.Property(i => i.SentById)
                .HasColumnOrder(2);

            builder.Property(i => i.RegardingId)
                .HasColumnOrder(3);

            builder.Property(i => i.Token)
                .HasColumnOrder(7)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(i => i.DataJson)
                .HasColumnOrder(8)
                .IsRequired(false)
                .HasMaxLength(450);

            builder.Property(i => i.ExpiresAt)
                .HasColumnOrder(10)
                .IsRequired();

            builder.HasOne(i => i.Regarding)
                .WithMany(p => p.Invitations)
                .HasForeignKey(i => i.RegardingId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasIndex(i => i.Token);
            builder.Ignore(i => i.Data);
        }
    }
}