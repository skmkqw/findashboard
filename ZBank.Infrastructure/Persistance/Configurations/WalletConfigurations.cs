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
        ConfigureBalancesTable(builder);
    }

    private void ConfigureWalletsTable(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable("Wallets");
        
        //PK
        builder.HasKey(x => x.Id);
        
        //ID
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        //ProfileId
        builder.Property(x => x.ProfileId)
            .ValueGeneratedNever();
        
        //Name
        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(50);
    }

    private void ConfigureBalancesTable(EntityTypeBuilder<Wallet> builder)
    {
        builder.OwnsMany(x => x.Balances, bb =>
        {
            bb.ToTable("WalletBalances");
            
            //FK
            bb.WithOwner().HasForeignKey("WalletId");

            //ID
            bb.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => BalanceId.Create(value));
            
            //Symbol
            bb.Property(x => x.Symbol)
                .HasMaxLength(10);
        });
        
        builder.Metadata.FindNavigation(nameof(Wallet.Balances))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}