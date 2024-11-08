using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZBank.Domain.ProjectAggregate;
using ZBank.Domain.ProjectAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;

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
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ProjectId.Create(value));
        
        //TeamId
        builder.Property(x => x.TeamId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => TeamId.Create(value));
        
        //Name
        builder.Property(x => x.Name)
            .HasMaxLength(100);
    }
}