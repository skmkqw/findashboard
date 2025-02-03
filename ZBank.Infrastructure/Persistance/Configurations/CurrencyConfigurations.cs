using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZBank.Domain.CurrencyAggregate;

namespace ZBank.Infrastructure.Persistance.Configurations;

public class CurrencyConfigurations : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable("Currencies");
        
        //PK
        builder.HasKey(x => x.Id);

        //ID
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        //Symbol
        builder.Property(x => x.Symbol)
            .IsRequired();
        
        //Price
        builder.Property(x => x.Price)
            .IsRequired();
    }
}