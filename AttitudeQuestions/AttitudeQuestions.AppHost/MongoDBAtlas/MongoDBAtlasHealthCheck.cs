using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AttitudeQuestions.AppHost.MongoDBAtlas;

public class MongoDBAtlasHealthCheck(IMongoClient client) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var adminDb = client.GetDatabase("admin");

        try
        {
            var initiateCmd = new BsonDocument { { "replSetInitiate", new BsonDocument() } };
            await adminDb.RunCommandAsync<BsonDocument>(initiateCmd, cancellationToken: cancellationToken);
        }
        catch (MongoCommandException ex)
        {
            if (ex.CodeName != "AlreadyInitialized")
            {
                return HealthCheckResult.Unhealthy("Failed to initialize MongoDB Atlas replica set.", ex);
            }
        }

        try
        {
            var isMasterCmd = new BsonDocument { { "isMaster", 1 } };
            var result = await adminDb.RunCommandAsync<BsonDocument>(isMasterCmd, cancellationToken: cancellationToken);
            if (result.TryGetValue("ismaster", out BsonValue isMaster) && isMaster.AsBoolean)
            {
                return HealthCheckResult.Healthy();
            }

            return HealthCheckResult.Unhealthy("MongoDB Atlas is not the primary node.");
        }
        catch (MongoCommandException ex)
        {
            return HealthCheckResult.Unhealthy("Failed to determine MongoDB Atlas primary node.", ex);
        }
    }
}
