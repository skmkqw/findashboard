using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZBank.Domain.TeamAggregate;

namespace ZBank.Infrastructure.Persistance.Configurations;

public class PersonalSpaceConfigurations : IEntityTypeConfiguration<PersonalSpace>
{
    public void Configure(EntityTypeBuilder<PersonalSpace> builder)
    {
        builder.ToTable("PersonalSpaces");
        
        //PK
        builder.HasKey(x => x.Id);
        
        //ID
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        //Name
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        //Description
        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasMaxLength(200);
        
        //OwnerId
        builder.Property(x => x.OwnerId)
            .ValueGeneratedNever();
    }
}