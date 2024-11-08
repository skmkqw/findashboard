using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZBank.Domain.WalletAggregate;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Infrastructure.Persistance.Configurations;

public class WalletConfigurations : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        ConfigureWalletsTable(builder);
    }
    
    public void ConfigureWalletsTable(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable("Wallets");
        
        //PK
        builder.HasKey(x => x.Id);
        
        //ID
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasConversion(
                id => id.Value,
                value => WalletId.Create(value));
        
        //Name
        builder.Property(x => x.Address)
            .HasMaxLength(50);
        
        //FK
        builder.HasOne(o => o.ProfileId)
            .WithMany()
            .HasForeignKey(o => o.ProfileId);
    }
}