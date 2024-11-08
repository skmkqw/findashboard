using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZBank.Domain.UserAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;

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
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));
        
        //FirstName
        builder.Property(x => x.FirstName)
            .HasMaxLength(100);
        
        //LastName
        builder.Property(x => x.LastName)
            .HasMaxLength(200);
        
        //Email
        builder.Property(x => x.Email)
            .HasMaxLength(100);
        
        //Password
        builder.Property(x => x.Password)
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
        builder.OwnsMany(r => r.TeamIds, pib =>
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
}