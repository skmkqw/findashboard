using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZBank.Domain.UserAggregate;

namespace ZBank.Infrastructure.Persistance.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUsersTable(builder);
        ConfigureProfileIdsTable(builder);
        ConfigureTeamIdsTable(builder);
    }

    private void ConfigureUsersTable(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        //PK
        builder.HasKey(x => x.Id);
        
        //ID
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        //FirstName
        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        
        //LastName
        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(200);
        
        //Email
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100);
        
        //Password
        builder.Property(x => x.Password)
            .IsRequired()
            .HasMaxLength(100);
    }
    
    private void ConfigureTeamIdsTable(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(r => r.TeamIds, tib =>
        {
            tib.ToTable("UserTeamIds");
            
            //FK
            tib.WithOwner().HasForeignKey("UserId");
            
            //PK
            tib.HasKey("Id");
            
            //ID
            tib.Property(d => d.Value)
                .ValueGeneratedNever()
                .HasColumnName("TeamId");

        });
        
        builder.Metadata.FindNavigation(nameof(User.TeamIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
    
    private void ConfigureProfileIdsTable(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(r => r.ProfileIds, pib =>
        {
            pib.ToTable("UserProfileIds");
            
            //FK
            pib.WithOwner().HasForeignKey("UserId");
            
            //PK
            pib.HasKey("Id");
            
            //ID
            pib.Property(d => d.Value)
                .ValueGeneratedNever()
                .HasColumnName("ProfileId");

        });
        
        builder.Metadata.FindNavigation(nameof(User.ProfileIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
    
    private void ConfigureNotificationIdsTable(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(n => n.NotificationIds, nb =>
        {
            nb.ToTable("UserNotificationIds");

            //FK
            nb.WithOwner().HasForeignKey("UserId");

            //PK
            nb.HasKey("Id");

            nb.Property(d => d.Value).ValueGeneratedNever().HasColumnName("NotificationId");
        });

        builder.Metadata.FindNavigation(nameof(User.NotificationIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}