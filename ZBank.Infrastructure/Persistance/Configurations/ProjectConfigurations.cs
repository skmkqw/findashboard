using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZBank.Domain.ProjectAggregate;

namespace ZBank.Infrastructure.Persistance.Configurations;

public class ProjectConfigurations : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        ConfigureProjectsTable(builder);
    }

    private void ConfigureProjectsTable(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");
        
        //PK
        builder.HasKey(x => x.Id);
        
        //ID
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        //TeamId
        builder.Property(x => x.TeamId)
            .ValueGeneratedNever();
        
        //Name
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}