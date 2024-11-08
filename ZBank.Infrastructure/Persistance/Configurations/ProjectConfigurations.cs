using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZBank.Domain.ProjectAggregate;
using ZBank.Domain.ProjectAggregate.ValueObjects;

namespace ZBank.Infrastructure.Persistance.Configurations;

public class ProjectConfigurations : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        ConfigureProjectsTable(builder);
    }
    
    public void ConfigureProjectsTable(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");
        
        //PK
        builder.HasKey(x => x.Id);
        
        //ID
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasConversion(
                id => id.Value,
                value => ProjectId.Create(value));
        
        //Name
        builder.Property(x => x.Name)
            .HasMaxLength(100);
        
        //FK
        builder.HasOne(o => o.TeamId)
            .WithMany()
            .HasForeignKey(o => o.TeamId);
    }
}