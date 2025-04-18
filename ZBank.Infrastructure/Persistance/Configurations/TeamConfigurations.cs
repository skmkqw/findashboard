using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZBank.Domain.TeamAggregate;

namespace ZBank.Infrastructure.Persistance.Configurations;

public class TeamConfigurations : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        ConfigureTeamsTable(builder);
        ConfigureUserIdsTable(builder);
        ConfigureProjectIdsTable(builder);
        ConfigureProfileIdsTable(builder);
        ConfigureActivityIdsTable(builder);
    }

    private void ConfigureTeamsTable(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Teams");
        
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
    }
    
    private void ConfigureUserIdsTable(EntityTypeBuilder<Team> builder)
    {
        builder.OwnsMany(r => r.UserIds, uib =>
        {
            uib.ToTable("TeamUserIds");
            
            //FK
            uib.WithOwner().HasForeignKey("TeamId");
            
            //PK
            uib.HasKey("Id");
            
            //ID
            uib.Property(d => d.Value)
                .ValueGeneratedNever()
                .HasColumnName("UserId");

        });
        
        builder.Metadata.FindNavigation(nameof(Team.UserIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
    
    private void ConfigureProfileIdsTable(EntityTypeBuilder<Team> builder)
    {
        builder.OwnsMany(r => r.ProfileIds, pib =>
        {
            pib.ToTable("ProfileIds");
            
            //FK
            pib.WithOwner().HasForeignKey("TeamId");
            
            //PK
            pib.HasKey("Id");
            
            //ID
            pib.Property(d => d.Value)
                .ValueGeneratedNever()
                .HasColumnName("ProfileId");

        });
        
        builder.Metadata.FindNavigation(nameof(Team.ProfileIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
    
    private void ConfigureProjectIdsTable(EntityTypeBuilder<Team> builder)
    {
        builder.OwnsMany(r => r.ProjectIds, pib =>
        {
            pib.ToTable("TeamProjectIds");
            
            //FK
            pib.WithOwner().HasForeignKey("TeamId");
            
            //PK
            pib.HasKey("Id");
            
            //ID
            pib.Property(d => d.Value)
                .ValueGeneratedNever()
                .HasColumnName("ProjectId");

        });
        
        builder.Metadata.FindNavigation(nameof(Team.ProjectIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
    
    private void ConfigureActivityIdsTable(EntityTypeBuilder<Team> builder)
    {
        builder.OwnsMany(r => r.ActivityIds, aib =>
        {
            aib.ToTable("TeamActivityIds");
            
            //FK
            aib.WithOwner().HasForeignKey("TeamId");
            
            //PK
            aib.HasKey("Id");
            
            //ID
            aib.Property(d => d.Value)
                .ValueGeneratedNever()
                .HasColumnName("ActivityId");

        });
        
        builder.Metadata.FindNavigation(nameof(Team.ActivityIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}