using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZBank.Domain.Common.Attributes;
using ZBank.Domain.Common.Models;

namespace ZBank.Infrastructure.Persistance.Extensions;

public static class ModelBuilderExtensions
{
    public static ModelBuilder AddStronglyTypedIdValueConverters<T>(
        this ModelBuilder modelBuilder)
    {
        var assembly = typeof(T).Assembly;
        foreach (var type in assembly.GetTypes())
        {
            var attribute = type
                .GetCustomAttributes<EfCoreValueConverterAttribute>()
                .FirstOrDefault();

            if (attribute is null)
            {
                continue;
            }

            var converter = (ValueConverter) Activator.CreateInstance(attribute.ValueConverter)!;

            modelBuilder.UseValueConverter(converter);
        }

        return modelBuilder;
    }

    private static ModelBuilder UseValueConverter(
        this ModelBuilder modelBuilder, ValueConverter converter)
    {
        var type = converter.ModelClrType;
        
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.IsOwned() && !(typeof(ValueObject).IsAssignableFrom(entityType.ClrType) && entityType.ClrType != typeof(ValueObject)))
            {
                continue;
            }
            
            var properties = entityType
                .ClrType
                .GetProperties()
                .Where(p => p.PropertyType == type);

            foreach (var property in properties)
            {
                modelBuilder
                    .Entity(entityType.Name)
                    .Property(property.Name)
                    .HasConversion(converter);
            }
        }

        return modelBuilder;
    }
}