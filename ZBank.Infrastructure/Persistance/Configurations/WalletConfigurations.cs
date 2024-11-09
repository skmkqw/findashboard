using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZBank.Domain.WalletAggregate;

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
            .ValueGeneratedNever();
        
        //ProfileId
        builder.Property(x => x.ProfileId)
            .ValueGeneratedNever();
        
        //Name
        builder.Property(x => x.Address)
            .HasMaxLength(50);
    }
}