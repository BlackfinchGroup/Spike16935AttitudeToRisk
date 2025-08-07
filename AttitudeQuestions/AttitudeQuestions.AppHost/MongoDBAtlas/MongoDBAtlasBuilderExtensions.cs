using System.Globalization;
using Aspire.Hosting.MongoDB;
using AttitudeQuestions.AppHost.MongoDBAtlas;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace AttitudeQuestions.AppHost.MongoDBAtlas;

public static class MongoDBAtlasBuilderExtensions
{
    // Internal port is always 27017.
    private const int DefaultContainerPort = 27017;

    private const string HostnameArgName = "--hostname";
    private const string UserEnvVarName = "MONGODB_INITDB_ROOT_USERNAME";
    private const string PasswordEnvVarName = "MONGODB_INITDB_ROOT_PASSWORD";

    /// <param name="builder">The <see cref="IDistributedApplicationBuilder"/>.</param>
    /// <param name="name">The name of the resource. This name will be used as the connection string name when referenced in a dependency.</param>
    /// <param name="port">The host port for MongoDB.</param>
    /// <param name="userName">A parameter that contains the MongoDb server user name, or <see langword="null"/> to use a default value.</param>
    /// <param name="password">A parameter that contains the MongoDb server password, or <see langword="null"/> to use a generated password.</param>
    /// <returns>A reference to the <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<MongoDBAtlasServerResource> AddMongoDBAtlas(this IDistributedApplicationBuilder builder,
        string name,
        int? port = null,
        IResourceBuilder<ParameterResource>? userName = null,
        IResourceBuilder<ParameterResource>? password = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(name);

        var passwordParameter = password?.Resource ?? ParameterResourceBuilderExtensions.CreateDefaultPasswordParameter(builder, $"{name}-password", special: false);

        var mongoDBAtlasContainer = new MongoDBAtlasServerResource(name, userName?.Resource, passwordParameter);

        string? connectionString = null;

        builder.Eventing.Subscribe<ConnectionStringAvailableEvent>(mongoDBAtlasContainer, async (@event, ct) =>
        {
            connectionString = await mongoDBAtlasContainer.ConnectionStringExpression.GetValueAsync(ct).ConfigureAwait(false);

            if (connectionString == null)
            {
                throw new DistributedApplicationException($"ConnectionStringAvailableEvent was published for the '{mongoDBAtlasContainer.Name}' resource but the connection string was null.");
            }
        });

        var healthCheckKey = $"{name}_check";
        // cache the client so it is reused on subsequent calls to the health check
        IMongoClient? client = null;
        builder.Services.AddHealthChecks()
            .AddMongoDBAtlas(
                sp => client ??= new MongoClient(connectionString ?? throw new InvalidOperationException("Connection string is unavailable")),
                name: healthCheckKey);

        return builder
            .AddResource(mongoDBAtlasContainer)
            .WithEndpoint(port: port, targetPort: DefaultContainerPort, name: MongoDBAtlasServerResource.PrimaryEndpointName)
            .WithImage(MongoDBAtlasContainerImageTags.Image, MongoDBAtlasContainerImageTags.Tag)
            .WithImageRegistry(MongoDBAtlasContainerImageTags.Registry)
            .WithContainerRuntimeArgs(HostnameArgName, mongoDBAtlasContainer.Name)
            .WithEnvironment(context =>
            {
                context.EnvironmentVariables[UserEnvVarName] = mongoDBAtlasContainer.UserNameReference;
                context.EnvironmentVariables[PasswordEnvVarName] = mongoDBAtlasContainer.PasswordParameter!;
            })
            .WithHealthCheck(healthCheckKey);
    }

    /// <summary>
    /// Adds a MongoDB Atlas database to the application model.
    /// </summary>
    /// <param name="builder">The MongoDB Atlas server resource builder.</param>
    /// <param name="name">The name of the resource. This name will be used as the connection string name when referenced in a dependency.</param>
    /// <param name="databaseName">The name of the database. If not provided, this defaults to the same value as <paramref name="name"/>.</param>
    /// <returns>A reference to the <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<MongoDBAtlasDatabaseResource> AddDatabase(this IResourceBuilder<MongoDBAtlasServerResource> builder, [ResourceName] string name, string? databaseName = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(name);

        // Use the resource name as the database name if it's not provided
        databaseName ??= name;

        builder.Resource.AddDatabase(name, databaseName);
        var mongoDBAtlasDatabase = new MongoDBAtlasDatabaseResource(name, databaseName, builder.Resource);

        string? connectionString = null;

        builder.ApplicationBuilder.Eventing.Subscribe<ConnectionStringAvailableEvent>(mongoDBAtlasDatabase, async (@event, ct) =>
        {
            connectionString = await mongoDBAtlasDatabase.ConnectionStringExpression.GetValueAsync(ct).ConfigureAwait(false);

            if (connectionString == null)
            {
                throw new DistributedApplicationException($"ConnectionStringAvailableEvent was published for the '{mongoDBAtlasDatabase.Name}' resource but the connection string was null.");
            }
        });

        var healthCheckKey = $"{name}_check";
        // cache the database client so it is reused on subsequent calls to the health check
        IMongoDatabase? database = null;
        builder.ApplicationBuilder.Services.AddHealthChecks()
            .AddMongoDb(
                sp => database ??=
                    new MongoClient(connectionString ?? throw new InvalidOperationException("Connection string is unavailable"))
                        .GetDatabase(databaseName),
                name: healthCheckKey);

        return builder.ApplicationBuilder
            .AddResource(mongoDBAtlasDatabase);
    }

    /// <summary>
    /// Adds a named volume for the data folder to a MongoDB Atlas container resource.
    /// </summary>
    /// <param name="builder">The resource builder.</param>
    /// <param name="name">The name of the volume. Defaults to an auto-generated name based on the application and resource names.</param>
    /// <param name="isReadOnly">A flag that indicates if this is a read-only volume.</param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<MongoDBAtlasServerResource> WithDataVolume(this IResourceBuilder<MongoDBAtlasServerResource> builder, string? name = null, bool isReadOnly = false)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder.WithVolume(name ?? VolumeNameGenerator.Generate(builder, "data"), "/data/db", isReadOnly);
    }

    /// <summary>
    /// Adds a named volume for the config folder to a MongoDB Atlas container resource.
    /// </summary>
    /// <param name="builder">The resource builder.</param>
    /// <param name="name">The name of the volume. Defaults to an auto-generated name based on the application and resource names.</param>
    /// <param name="isReadOnly">A flag that indicates if this is a read-only volume.</param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<MongoDBAtlasServerResource> WithConfigVolume(this IResourceBuilder<MongoDBAtlasServerResource> builder, string? name = null, bool isReadOnly = false)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder.WithVolume(name ?? VolumeNameGenerator.Generate(builder, "config"), "/data/configdb", isReadOnly);
    }

    /// <summary>
    /// Adds a named volume for the search (mongot) folder to a MongoDB Atlas container resource.
    /// </summary>
    /// <param name="builder">The resource builder.</param>
    /// <param name="name">The name of the volume. Defaults to an auto-generated name based on the application and resource names.</param>
    /// <param name="isReadOnly">A flag that indicates if this is a read-only volume.</param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<MongoDBAtlasServerResource> WithSearchVolume(this IResourceBuilder<MongoDBAtlasServerResource> builder, string? name = null, bool isReadOnly = false)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder.WithVolume(name ?? VolumeNameGenerator.Generate(builder, "search"), "/data/mongot", isReadOnly);
    }

    /// <summary>
    /// Adds a MongoExpress administration and development platform for MongoDB to the application model.
    /// </summary>
    /// <remarks>
    /// This version of the package defaults to the <inheritdoc cref="MongoDBAtlasContainerImageTags.MongoExpressTag"/> tag of the <inheritdoc cref="MongoDBAtlasContainerImageTags.MongoExpressImage"/> container image.
    /// </remarks>
    /// <param name="builder">The MongoDB Atlas server resource builder.</param>
    /// <param name="configureContainer">Configuration callback for Mongo Express container resource.</param>
    /// <param name="containerName">The name of the container (Optional).</param>
    /// <returns>A reference to the <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<T> WithMongoExpress<T>(this IResourceBuilder<T> builder, Action<IResourceBuilder<MongoExpressContainerResource>>? configureContainer = null, string? containerName = null) where T : MongoDBAtlasServerResource
    {
        ArgumentNullException.ThrowIfNull(builder);

        containerName ??= $"{builder.Resource.Name}-mongoexpress";

        var mongoExpressContainer = new MongoExpressContainerResource(containerName);
        var resourceBuilder = builder.ApplicationBuilder.AddResource(mongoExpressContainer)
                                                        .WithImage(MongoDBAtlasContainerImageTags.MongoExpressImage, MongoDBAtlasContainerImageTags.MongoExpressTag)
                                                        .WithImageRegistry(MongoDBAtlasContainerImageTags.MongoExpressRegistry)
                                                        .WithEnvironment(context => ConfigureMongoExpressContainer(context, builder.Resource))
                                                        .WithHttpEndpoint(targetPort: 8081, name: "http")
                                                        .ExcludeFromManifest();

        configureContainer?.Invoke(resourceBuilder);

        return builder;
    }

    private static void ConfigureMongoExpressContainer(EnvironmentCallbackContext context, MongoDBAtlasServerResource resource)
    {
        // url is used for initial health check
        context.EnvironmentVariables.Add("ME_CONFIG_MONGODB_URL", $"{resource.Name}:{resource.PrimaryEndpoint.TargetPort ?? 27017}");
        context.EnvironmentVariables.Add("ME_CONFIG_MONGODB_SERVER", resource.Name);
        var targetPort = resource.PrimaryEndpoint.TargetPort;
        if (targetPort is int targetPortValue)
        {
            context.EnvironmentVariables.Add("ME_CONFIG_MONGODB_PORT", targetPortValue.ToString(CultureInfo.InvariantCulture));
        }
        context.EnvironmentVariables.Add("ME_CONFIG_BASICAUTH", "false");
        if (resource.PasswordParameter is not null)
        {
            context.EnvironmentVariables.Add("ME_CONFIG_MONGODB_ADMINUSERNAME", resource.UserNameReference);
            context.EnvironmentVariables.Add("ME_CONFIG_MONGODB_ADMINPASSWORD", resource.PasswordParameter);
        }
    }
}
