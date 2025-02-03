using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZBank.Domain.Common.Models;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Infrastructure.Persistance.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        //PK
        builder.HasKey(n => n.Id);
        
        //ID
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        //Discriminator
        builder.HasDiscriminator<string>("NotificationType")
            .HasValue<InformationNotification>("Information")
            .HasValue<TeamInviteNotification>("TeamInvite");

        //Common properties
        builder.Property(n => n.Content).IsRequired();
        builder.Property(n => n.IsRead).IsRequired();
        
        builder.OwnsOne(n => n.NotificationSender, nb =>
        {
            //SenderFullName
            nb.Property(ns => ns.SenderFullName).HasColumnName("SenderFullName")
                .IsRequired()
                .HasMaxLength(200);
            
            //SenderId
            nb.Property(ns => ns.SenderId)
                .ValueGeneratedNever()
                .HasColumnName("SenderId")
                .HasConversion(
                    id => id.Value,
                    value => UserId.Create(value)
                );
        });
    }
}