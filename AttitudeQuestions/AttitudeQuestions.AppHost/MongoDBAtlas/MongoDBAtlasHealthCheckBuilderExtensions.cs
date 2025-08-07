using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;

namespace AttitudeQuestions.AppHost.MongoDBAtlas;

public static class MongoDBAtlasHealthCheckBuilderExtensions
{
    public static IHealthChecksBuilder AddMongoDBAtlas(
        this IHealthChecksBuilder builder,
        Func<IServiceProvider, IMongoClient>? clientFactory = default,
        string? name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        return builder.Add(new HealthCheckRegistration(
            name ?? "mongodb-atlas",
            sp => Factory(sp, clientFactory),
            failureStatus,
            tags,
            timeout));

        static MongoDBAtlasHealthCheck Factory(IServiceProvider sp, Func<IServiceProvider, IMongoClient>? clientFactory)
        {
            // The user might have registered a factory for MongoClient type, but not for the abstraction (IMongoClient).
            // That is why we try to resolve MongoClient first.
            IMongoClient client = clientFactory?.Invoke(sp) ?? sp.GetService<MongoClient>() ?? sp.GetRequiredService<IMongoClient>();
            return new(client);
        }
    }
}
