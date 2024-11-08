using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Infrastructure.Persistance.Configurations;

public class WalletConfigurations : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        ConfigureWalletsTable(builder);
    }

    private void ConfigureWalletsTable(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable("Wallets");
        
        //PK
        builder.HasKey(x => x.Id);
        
        //ID
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => WalletId.Create(value));
        
        //ProfileId
        builder.Property(x => x.ProfileId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ProfileId.Create(value));
        
        //Name
        builder.Property(x => x.Address)
            .HasMaxLength(50);
    }
}