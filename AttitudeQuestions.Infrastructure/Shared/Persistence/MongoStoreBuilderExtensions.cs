using AttitudeQuestions.Application.Shared;
using AttitudeQuestions.Domain;
using AttitudeQuestions.Infrastructure.QuestionSetSessions;
using AttitudeQuestions.Infrastructure.Shared.Persistence.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace AttitudeQuestions.Infrastructure.Shared.Persistence;

public static class MongoStoreBuilderExtensions
{
    public static MongoStoreBuilder AddMongo(this IHostApplicationBuilder builder, string connectionStringName = "questions", string configSectionName = "RepositoryOptions", bool addHealthCheck = true, bool registerSerialiser = true)
    {
        if (registerSerialiser)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        }

        builder.Services.AddOptionsWithValidateOnStart<RepositoryOptions>()
            .BindConfiguration(configSectionName)
            .ValidateDataAnnotations();

        if (builder.Configuration.GetConnectionString(connectionStringName) is string connectionString)
        {
            builder
                  .Services
                  .AddSingleton<IMongoClient>(sp => new MongoClient(MongoClientSettings.FromConnectionString(connectionString)));

            if (addHealthCheck)
            {
                builder.Services.AddHealthChecks().AddMongoDb();
            }
        }

        return new MongoStoreBuilder(builder);
    }

    public static MongoStoreBuilder AddQuestionStore(this MongoStoreBuilder storeBuilder)
    {
        storeBuilder.Builder.Services.AddDbContext<QuestionDbContext>();

        storeBuilder.Builder.Services.AddScoped<IRepository<QuestionSetSession>, QuestionSetSessionRepository>();

        return storeBuilder;
    }
}

public class MongoStoreBuilder(IHostApplicationBuilder builder)
{
    public IHostApplicationBuilder Builder { get; } = builder;
}
