using DigitalWallet.Shared.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalWallet.Shared.Infrastructure.Persistence.Configuration;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Type).IsRequired();
        
        builder.Property(x => x.Payload)
            .IsRequired()
            .HasColumnType("jsonb"); 
            
        builder.Property(x => x.CreatedAt).IsRequired();
        
        builder.ToTable("outbox_messages");
    }
}