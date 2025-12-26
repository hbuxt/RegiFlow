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

            builder.Property(i => i.RelatesToId)
                .HasColumnOrder(3);

            builder.Property(i => i.Token)
                .HasColumnOrder(7)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(i => i.ExpiresAt)
                .HasColumnOrder(9)
                .IsRequired();

            builder.HasOne(i => i.RelatesTo)
                .WithMany(p => p.Invitations)
                .HasForeignKey(i => i.RelatesToId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasIndex(i => i.Token);
        }
    }
}