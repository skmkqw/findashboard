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
    }

    public void ConfigureUsersTable(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        //PK
        builder.HasKey(x => x.Id);
        
        //ID
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
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
}