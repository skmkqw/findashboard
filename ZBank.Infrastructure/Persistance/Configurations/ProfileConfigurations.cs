using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZBank.Domain.ProfileAggregate;
using ZBank.Domain.ProfileAggregate.ValueObjects;

namespace ZBank.Infrastructure.Persistance.Configurations;

public class ProfileConfigurations : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        ConfigureProfilesTable(builder);
        ConfigureWalletIdsTable(builder);
    }
    
    public void ConfigureProfilesTable(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("Profiles");
        
        //PK
        builder.HasKey(x => x.Id);
        
        //ID
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasConversion(
                id => id.Value,
                value => ProfileId.Create(value));
        
        //Name
        builder.Property(x => x.Name)
            .HasMaxLength(100);
    }

    private void ConfigureWalletIdsTable(EntityTypeBuilder<Profile> builder)
    {
        builder.OwnsMany(r => r.WalletIds, wib =>
        {
            wib.ToTable("ProfileWalletIds");
            
            //FK
            wib.WithOwner().HasForeignKey("ProfileId");
            
            //PK
            wib.HasKey("Id");
            
            //ID
            wib.Property(d => d.Value)
                .ValueGeneratedNever()
                .HasColumnName("WalletId");

        });
        
        builder.Metadata.FindNavigation(nameof(Profile.WalletIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}