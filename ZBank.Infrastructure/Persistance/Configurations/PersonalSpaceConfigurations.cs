using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZBank.Domain.TeamAggregate;

namespace ZBank.Infrastructure.Persistance.Configurations;

public class PersonalSpaceConfigurations : IEntityTypeConfiguration<PersonalSpace>
{
    public void Configure(EntityTypeBuilder<PersonalSpace> builder)
    {
        ConfigurePersonalSpacesTable(builder);
        ConfigureProjectIdsTable(builder);
        ConfigureProfileIdsTable(builder);
        ConfigureActivityIdsTable(builder);
    }

    private void ConfigurePersonalSpacesTable(EntityTypeBuilder<PersonalSpace> builder)
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
    
    private void ConfigureProjectIdsTable(EntityTypeBuilder<PersonalSpace> builder)
    {
        builder.OwnsMany(r => r.ProjectIds, pib =>
        {
            pib.ToTable("PersonalProjectIds");
            
            //FK
            pib.WithOwner().HasForeignKey("PersonalSpaceId");
            
            //PK
            pib.HasKey("Id");
            
            //ID
            pib.Property(d => d.Value)
                .ValueGeneratedNever()
                .HasColumnName("ProjectId");

        });
        
        builder.Metadata.FindNavigation(nameof(PersonalSpace.ProjectIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
    
    private void ConfigureProfileIdsTable(EntityTypeBuilder<PersonalSpace> builder)
    {
        builder.OwnsMany(r => r.ProfileIds, pib =>
        {
            pib.ToTable("PersonalProfileIds");
            
            //FK
            pib.WithOwner().HasForeignKey("PersonalSpaceId");
            
            //PK
            pib.HasKey("Id");
            
            //ID
            pib.Property(d => d.Value)
                .ValueGeneratedNever()
                .HasColumnName("ProfileId");

        });
        
        builder.Metadata.FindNavigation(nameof(PersonalSpace.ProfileIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
    
    private void ConfigureActivityIdsTable(EntityTypeBuilder<PersonalSpace> builder)
    {
        builder.OwnsMany(r => r.ActivityIds, aib =>
        {
            aib.ToTable("PersonalActivityIds");
            
            //FK
            aib.WithOwner().HasForeignKey("PersonalSpaceId");
            
            //PK
            aib.HasKey("Id");
            
            //ID
            aib.Property(d => d.Value)
                .ValueGeneratedNever()
                .HasColumnName("ActivityId");

        });
        
        builder.Metadata.FindNavigation(nameof(PersonalSpace.ActivityIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}