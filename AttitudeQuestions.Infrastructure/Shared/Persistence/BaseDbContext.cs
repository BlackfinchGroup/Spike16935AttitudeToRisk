using System.Reflection;
using AttitudeQuestions.Infrastructure.Shared.Persistence.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AttitudeQuestions.Infrastructure.Shared.Persistence;

public class BaseDbContext<T>(IMongoClient mongoClient, IOptions<RepositoryOptions> repositoryOptions, IHostEnvironment environment) : DbContext
{
    private readonly string _databaseName = repositoryOptions.Value.PrimaryDbName;

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        RegisterValueTypeValueConverters(configurationBuilder, InfrastructureProject.Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMongoDB(mongoClient, _databaseName);
        optionsBuilder.ConfigureWarnings(x => x.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning));
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Warning);
        optionsBuilder.EnableSensitiveDataLogging(environment.IsDevelopment());
    }

    protected static void RegisterValueTypeValueConverters(ModelConfigurationBuilder configurationBuilder, Assembly assembly) =>
        assembly.GetTypes()
            .Where(x =>
                !x.IsAbstract
                && x.BaseType is not null
                && x.BaseType.IsGenericType
                && x.BaseType.GetGenericTypeDefinition() == typeof(ValueConverter<,>))
            .ToList()
            .ForEach(valueConverterType =>
            {
                Type valueObjectType = valueConverterType.BaseType!.GetGenericArguments().First();
                configurationBuilder.Properties(valueObjectType)
                    .HaveConversion(valueConverterType);
            });
}
