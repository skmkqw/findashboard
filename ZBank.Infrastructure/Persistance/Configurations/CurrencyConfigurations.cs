using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZBank.Domain.CurrencyAggregate;

namespace ZBank.Infrastructure.Persistance.Configurations;

public class CurrencyConfigurations : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable("Currencies");
        
        //PK (Symbol)
        builder.HasKey(x => x.Id)
            .HasName("Symbol");

        //ID
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        //Price
        builder.Property(x => x.Price)
            .IsRequired();
        
        //PriceChangeIn24Hours
        builder.Property(x => x.PriceChangeIn24Hours)
            .IsRequired();
    }
}